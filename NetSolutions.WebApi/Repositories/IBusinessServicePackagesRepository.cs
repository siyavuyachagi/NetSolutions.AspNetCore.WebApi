using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories;

public interface IBusinessServicePackagesRepository
{
    Task<Result<List<BusinessServicePackageDto>>> GetBusinessServicePackagesAsync();
    Task<Result<BusinessServicePackageDto?>> GetBusinessServicePackageAsync(Guid Id);
    Task<Result> DeleteBusinessServicePackageAsync(Guid Id);
}


public class BusinessServicePackagesRepository : IBusinessServicePackagesRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationUserRepository> _logger;
    private readonly IRedisCache _redisCache;
    private const string BUSINESS_SERVICE_PACKAGES_CACHE_KEY = "business_service_packages_list_cache";

    public BusinessServicePackagesRepository(
        ApplicationDbContext context, 
        ILogger<ApplicationUserRepository> logger, 
        IRedisCache redisCache)
    {
        _context = context;
        _logger = logger;
        _redisCache = redisCache;
    }

    public async Task<Result> DeleteBusinessServicePackageAsync(Guid Id)
    {
        try
        {
            await _context.BusinessServicePackages
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

    public async Task<Result<BusinessServicePackageDto?>> GetBusinessServicePackageAsync(Guid Id)
    {
        try
        {
            var businessServicePackagesCache = await _redisCache.GetAsync<List<BusinessServicePackageDto>>(BUSINESS_SERVICE_PACKAGES_CACHE_KEY) ?? [];
            if (businessServicePackagesCache.Count != 0)
            {
                return Result.Success(businessServicePackagesCache.Where(x => x.Id == Id).FirstOrDefault());
            }
            else
            {
                var businessServicePackages = await RefreshCacheAsync();
                return Result.Success(businessServicePackages.Where(x => x.Id == Id).FirstOrDefault());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return (Result<BusinessServicePackageDto?>)Result.Failed(ex.Message);
        }
    }

    public async Task<Result<List<BusinessServicePackageDto>>> GetBusinessServicePackagesAsync()
    {
        try
        {
            var businessServicePackagesCache = await _redisCache.GetAsync<List<BusinessServicePackageDto>>(BUSINESS_SERVICE_PACKAGES_CACHE_KEY) ?? [];
            if (businessServicePackagesCache.Count != 0)
            {
                return Result.Success(businessServicePackagesCache);
            }
            else
            {
                var businessServicePackages = await RefreshCacheAsync();
                return Result.Success(businessServicePackages);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return (Result<List<BusinessServicePackageDto>>)Result.Failed(ex.Message);
        }
    }

    public async Task<List<BusinessServicePackageDto>> RefreshCacheAsync()
    {
        try
        {
            var businessServicePackagesDto = await _context.BusinessServicePackages
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(s => new BusinessServicePackageDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    BusinessService = s.BusinessService,
                    BusinessServicePackageFeatures = s.PackageFeatures,
                    BillingCycle = EnumHelper.GetDisplayName(s.BillingCycle),
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                })
                .ToListAsync();

            if (businessServicePackagesDto.Any())
                await _redisCache.SetAsync(BUSINESS_SERVICE_PACKAGES_CACHE_KEY, businessServicePackagesDto);

            return businessServicePackagesDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return [];
        }
    }
}
