using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NetSolutions.WebApi.Models.Domain;
using NetSolutions.WebApi.Data;

namespace NetSolutions.WebApi.Services;

public interface IRedisCache : IHostedService, IDisposable
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}

public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCache> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ApplicationDbContext _context;
    private Timer _timer;

    // Cache keys
    private const string SOLUTIONS_CACHE_KEY = "solutions_all";
    private const string PROJECT_PREFIX = "project_";

    public RedisCache(
        IDistributedCache cache,
        ILogger<RedisCache> logger,
        IServiceProvider serviceProvider,
        ApplicationDbContext context
    )
    {
        _cache = cache;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _context = context;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(RefreshData, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void RefreshData(object state)
    {
        try
        {
            // Use a scope to get the DbContext or any other scoped service
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await RefreshSolutionsCache(dbContext);

            _logger.LogInformation("Redis cache refreshed at {time}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing Redis cache");
        }
    }

    private async Task RefreshSolutionsCache(ApplicationDbContext context)
    {
        var solutions = await context.Solutions
            .AsNoTrackingWithIdentityResolution()
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.ProjectId,
                s.Project,
                s.Description,
                s.Price,
                s.SourceUrl,
                s.PreviewUrl,
                s.Version,
                s.CreatedAt,
                s.UpdatedAt,
                Features = s.SolutionFeatures,
                TechnologyStacks = s.TechnologyStacks.Select(x => x.TechnologyStack).ToList(),
                Images = s.Images.Select(i => i.FileMetadata).ToList(),
                Documents = s.Documents.Select(x => x.FileMetadata).ToList(),
                Reviews = s.Reviews.Select(x => x.Review).ToList(),
                Discriminator = EF.Property<string>(s, "Discriminator").ToFormattedString(Casing.Pascal).Replace(nameof(Solution), ""),
            })
            .ToListAsync();

        // Cache all solutions
        string serializedData = JsonSerializer.Serialize(solutions);
        await _cache.SetStringAsync(SOLUTIONS_CACHE_KEY, serializedData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedData = await _cache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cachedData))
        {
            try
            {
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing cached data for key {key}", key);
                // Fall through to re-create the cache
            }
        }

        // Cache miss or deserialization error - execute factory
        var data = await factory();

        // Store in cache
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
        };

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), options);

        return data;
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _cache.GetStringAsync(key) != null;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}