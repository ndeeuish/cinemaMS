using System.Text.Json;
using CinemaMS.Application.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace CinemaMS.Infrastructure.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedData = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedData);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions();

        if (absoluteExpireTime.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime;
        }

        if (slidingExpireTime.HasValue)
        {
            options.SlidingExpiration = slidingExpireTime;
        }

        var serializedData = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, serializedData, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
