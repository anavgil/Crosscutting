using Microsoft.Extensions.Caching.Distributed;

namespace Crosscutting.Cache.Services.Abstraction;

public interface ICacheService
{
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options);
    void Set<T>(string key, T value, DistributedCacheEntryOptions options);
    Task<T> TryGetValueAsync<T>(string key, CancellationToken ct = default);
    bool TryGetValue<T>(string key, out T value);
    void RemoveKey(string key);
    Task RemoveKeyAsync(string key);
}
