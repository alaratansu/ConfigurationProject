using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Configuration.Infrastructure.Services;

public class RedisCacheService :IRedisCacheService
{
    private readonly IDistributedCache cache;
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public RedisCacheService(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpire = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpire ?? TimeSpan.FromMinutes(30)
        };

        var json = JsonSerializer.Serialize(value, jsonOptions);
        await cache.SetStringAsync(key, json, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await cache.GetStringAsync(key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json, jsonOptions);
    }
}