using Microsoft.EntityFrameworkCore;
using NetSolutions.Helpers;
using NetSolutions.WebApi.Data;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Models.DTOs;
using NetSolutions.WebApi.Services;

namespace NetSolutions.WebApi.Repositories;

public interface ISolutionsRepository
{
    Task<Result<List<SolutionDto>>> GetSolutionsAsync();
    Task<Result<SolutionDto?>> GetSolutionAsync(Guid Id);
    Task<Result> DeleteSolutionAsync(Guid Id);
}

public class SolutionsRepository : ISolutionsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationUserRepository> _logger;
    private readonly IRedisCache _redisCache;
    private const string SOLUTIONS_CACHE_KEY = "solutions_list_cache";

    public SolutionsRepository(
        ApplicationDbContext context,
        ILogger<ApplicationUserRepository> logger,
        IRedisCache redisCache)
    {
        _context = context;
        _logger = logger;
        _redisCache = redisCache;
    }

    public async Task<Result<SolutionDto?>> GetSolutionAsync(Guid Id)
    {
        try
        {
            var solutionsCache = await _redisCache.GetAsync<List<SolutionDto>>(SOLUTIONS_CACHE_KEY) ?? [];
            if (solutionsCache.Count != 0)
            {
                var solution = solutionsCache.Where(p => p.Id == Id).FirstOrDefault();
                return Result.Success(solution);
            }
            else
            {
                var solutions = await RefreshCacheAsync();
                var solution = solutions.Where(p => p.Id == Id).FirstOrDefault();
                return Result.Success(solution);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failed<SolutionDto?>(ex.Message);
        }
    }

    public async Task<Result<List<SolutionDto>>> GetSolutionsAsync()
    {
        try
        {
            var solutionsCache = await _redisCache.GetAsync<List<SolutionDto>>(SOLUTIONS_CACHE_KEY) ?? [];
            if (solutionsCache.Count != 0)
            {
                return Result.Success(solutionsCache);
            }
            else
            {
                var solutions = await RefreshCacheAsync();
                return Result.Success(solutions);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await _redisCache.RemoveAsync(SOLUTIONS_CACHE_KEY);
            return Result.Failed<List<SolutionDto>>(ex.Message);
        }
    }

    public async Task<Result> DeleteSolutionAsync(Guid Id)
    {
        try
        {
            await _context.Solutions
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


    public async Task<List<SolutionDto>> RefreshCacheAsync()
    {
        try
        {
            var solutions = await _context.Solutions
                .AsNoTrackingWithIdentityResolution()
                .Where(x => !x.IsDeleted)
                .Select(s => new SolutionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Project = s.Project,
                    Description = s.Description,
                    Price = s.Price,
                    SourceUrl = s.SourceUrl,
                    PreviewUrl = s.PreviewUrl,
                    Version = s.Version,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    Features = s.SolutionFeatures.ToList(),
                    TechnologyStacks = s.TechnologyStacks.Select(x => x.TechnologyStack).ToList(),
                    Images = s.Images.Select(i => i.FileMetadata).ToList(),
                    Documents = s.Documents.Select(x => x.FileMetadata).ToList(),
                    Reviews = s.Reviews.Select(x => x.Review).ToList(),
                    Likes = s.Likes,
                    Discriminator = EF.Property<string>(s, "Discriminator")
                        .ToFormattedString(Casing.Pascal)
                        .Replace(nameof(Solution), "")
                })
                .ToListAsync();

            //Save cache
            if (solutions.Any())
                await _redisCache.SetAsync(SOLUTIONS_CACHE_KEY, solutions);

            return solutions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return [];
        }
    }
}
