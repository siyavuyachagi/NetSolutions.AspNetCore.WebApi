using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace NetSolutions.WebApi.Services;

public interface IRedisCache
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
}


public class RedisCache : IRedisCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCache> _logger;

    public RedisCache(
        IDistributedCache cache,
        ILogger<RedisCache> logger
    )
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _cache.GetStringAsync(key) != null;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cachedValue = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedValue))
                return default;

            return JsonConvert.DeserializeObject<T>(cachedValue);
        }
        catch (Exception ex)
        {
            _cache.Remove(key);
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30),
            };
            // Store in cache

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}