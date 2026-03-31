using System.Text.Json;
using EcommerceAPI.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace EcommerceAPI.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        if (data is null) return default;
        return JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
        };
        var data = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, data, options);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
