using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TaskManager.Caching;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _cache;
    public RedisService(IDistributedCache cache) => _cache = cache;

    public T? GetData<T>(string key)
    {
        var data = _cache?.GetString(key);
        if (data is null)
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        };
        _cache?.SetString(key, JsonSerializer.Serialize(data), options);
    }
}