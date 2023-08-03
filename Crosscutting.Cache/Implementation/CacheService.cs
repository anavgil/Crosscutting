using Crosscutting.Cache.Abstraction;
using Crosscutting.Cache.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Crosscutting.Cache.Implementation;


public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options)
    {
        await _cache.SetAsync(key, value, options);
    }

    public void Set<T>(string key, T value, DistributedCacheEntryOptions options)
    {
        _cache.Set(key, value, options);
    }

    public async Task<T> TryGetValueAsync<T>(string key, CancellationToken ct = default)
    {
        return await _cache.TryGetValueAsync<T>(key, ct);
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void RemoveKey(string key)
    {
        _cache.Remove(key);
    }

    public async Task RemoveKeyAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
