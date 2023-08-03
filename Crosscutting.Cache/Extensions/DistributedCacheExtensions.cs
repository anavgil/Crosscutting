using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Crosscutting.Cache.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions());
        }

        public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));
            return cache.SetAsync(key, bytes, options);
        }

        public static void Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));
            cache.Set(key, bytes, options);
        }

        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T value)
        {
            var val = cache.Get(key);
            value = default;
            if (val == null) return false;
            value = JsonSerializer.Deserialize<T>(val, SerializerOptions);
            return true;
        }

        public static async Task<T> TryGetValueAsync<T>(this IDistributedCache cache, string key, CancellationToken ct = default)
        {
            var val = await cache.GetAsync(key, ct);
            if (val == null) return default;
            var value = JsonSerializer.Deserialize<T>(val, SerializerOptions);
            return value;
        }

        private static JsonSerializerOptions SerializerOptions => new()
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }
}
