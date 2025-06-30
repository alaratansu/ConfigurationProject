namespace Configuration.Infrastructure.Services;

public interface IRedisCacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpire = null);
    Task<T?> GetAsync<T>(string key);
}