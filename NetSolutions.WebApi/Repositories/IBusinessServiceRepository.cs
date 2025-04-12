using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories;

public interface IBusinessServiceRepository
{
    Task<Result<List<BusinessServiceDto>>> GetBusinessServicesAsync();
    Task<Result<BusinessServiceDto?>> GetBusinessServiceAsync(Guid Id);
    Task<Result> DeleteBusinessServiceAsync(Guid Id);
}


public class BusinessServiceRepository : IBusinessServiceRepository
{
    private const string BUSINESS_SERVICE_CACHE_KEY = "business_service_list_cache";
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationUserRepository> _logger;
    private readonly IRedisCache _redisCache;

    public BusinessServiceRepository(
        ApplicationDbContext context,
        ILogger<ApplicationUserRepository> logger,
        IRedisCache redisCache)
    {
        _context = context;
        _logger = logger;
        _redisCache = redisCache;
    }

    public async Task<Result> DeleteBusinessServiceAsync(Guid Id)
    {
        try
        {
            await _context.BusinessServices
            .Where(u => u.Id == Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.IsDeleted, true)
                .SetProperty(u => u.UpdatedAt, DateTime.Now));

            //Refresh cache
            await RefreshCacheAsync();

            //Return success
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failed(ex.Message);
        }
    }

    public async Task<Result<BusinessServiceDto?>> GetBusinessServiceAsync(Guid Id)
    {
        try
        {
            var businessServicesCache = await _redisCache.GetAsync<List<BusinessServiceDto>>(BUSINESS_SERVICE_CACHE_KEY) ?? [];
            if (businessServicesCache.Count != 0)
            {
                return Result.Success(businessServicesCache.Where(x => x.Id == Id).FirstOrDefault());
            }
            else
            {
                var businessServices = await RefreshCacheAsync();
                return Result.Success(businessServices.Where(x => x.Id == Id).FirstOrDefault());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return (Result<BusinessServiceDto?>)Result.Failed(ex.Message);
        }
    }

    public async Task<Result<List<BusinessServiceDto>>> GetBusinessServicesAsync()
    {
        try
        {
            var businessServicesCache = await _redisCache.GetAsync<List<BusinessServiceDto>>(BUSINESS_SERVICE_CACHE_KEY) ?? [];
            if (businessServicesCache.Count != 0)
            {
                return Result.Success(businessServicesCache);
            }
            else
            {
                var businessServices = await RefreshCacheAsync();
                return Result.Success(businessServices);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return (Result<List<BusinessServiceDto>>)Result.Failed(ex.Message);
        }
    }

    public async Task<List<BusinessServiceDto>> RefreshCacheAsync()
    {
        try
        {
            var businessServices = await _context.BusinessServices
            .AsNoTracking()
            .Include(bs => bs.Testimonials)
                .ThenInclude(t => t.Testimonial.Evaluator)
            .Include(bs => bs.Packages)
                .ThenInclude(p => p.PackageFeatures)
            .Include(bs => bs.Packages)
                .ThenInclude(p => p.Subscriptions)
            .Select(s => new BusinessServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Testimonials = s.Testimonials.Select(t => t.Testimonial).ToList(),
                Thumbnail = s.Thumbnail.FileMetadata.ViewLink,
                BusinessServicePackages = s.Packages.Select(pkg => new BusinessServicePackageDto
                {
                    Id = pkg.Id,
                    Name = pkg.Name,
                    Price = pkg.Price,
                    BillingCycle = EnumHelper.GetDisplayName(pkg.BillingCycle),
                    Description = pkg.Description,
                    BusinessServiceId = pkg.BusinessServiceId,
                    BusinessServicePackageFeatures = pkg.PackageFeatures,
                    CreatedAt = pkg.CreatedAt,
                    Subscriptions = pkg.Subscriptions.Select(s => new SubscriptionDto
                    {
                        Id = s.Id,
                        Client = new ClientDto
                        {
                            Id = s.Client.Id,
                            UserName = s.Client.UserName,
                            Email = s.Client.Email,
                            LastName = s.Client.LastName,
                            FirstName = s.Client.FirstName,
                            Avatar = s.Client.Avatar,
                            PhoneNumber = s.Client.PhoneNumber,
                            EmailConfirmed = s.Client.EmailConfirmed,
                        },
                        BusinessServicePackage = new BusinessServicePackageDto
                        {
                            Id = s.BusinessServicePackage.Id,
                            Name = s.BusinessServicePackage.Name,
                            Description = s.BusinessServicePackage.Description,
                            Price = s.BusinessServicePackage.Price,
                            BusinessService = s.BusinessServicePackage.BusinessService,
                            BusinessServicePackageFeatures = s.BusinessServicePackage.PackageFeatures,
                            BillingCycle = EnumHelper.GetDisplayName(s.BusinessServicePackage.BillingCycle),
                            CreatedAt = s.BusinessServicePackage.CreatedAt,
                            UpdatedAt = s.BusinessServicePackage.UpdatedAt,
                        },
                        Status = EnumHelper.GetDisplayName(s.Status),
                        CreatedAt = s.CreatedAt,
                        RecurringCycle = s.RecurringCycle,
                        UpdatedAt = s.UpdatedAt,
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();

            if (businessServices.Any())
                await _redisCache.SetAsync(BUSINESS_SERVICE_CACHE_KEY, businessServices);

            return businessServices;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return [];
        }
    }
}
