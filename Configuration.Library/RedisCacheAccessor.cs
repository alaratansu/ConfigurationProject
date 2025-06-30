using Configuration.Infrastructure.Services;

namespace Configuration.Library;

public static class RedisCacheAccessor
{
    public static IRedisCacheService? Instance { get; set; }
}