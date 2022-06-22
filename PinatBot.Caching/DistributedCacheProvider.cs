using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace PinatBot.Caching;

public class DistributedCacheProvider
{
    public DistributedCacheProvider(IDistributedCache distributedCache, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
    {
        DistributedCache = distributedCache;
        JsonSerializerOptions = jsonOptions.Get("Discord");
    }

    private IDistributedCache DistributedCache { get; }
    private JsonSerializerOptions JsonSerializerOptions { get; }

    public async Task CacheAsync<TInstance>(string key, TInstance instance, CancellationToken ct = default)
        where TInstance : class
    {
        var serialized = JsonSerializer.Serialize(instance, JsonSerializerOptions);
        await DistributedCache.SetStringAsync(key, serialized, ct);
    }

    public async Task<TInstance?> RetrieveAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        var value = await DistributedCache.GetAsync(key, ct);
        return value is null ? null : JsonSerializer.Deserialize<TInstance>(value, JsonSerializerOptions);
    }

    public async Task EvictAsync(string key, CancellationToken ct = default) => await DistributedCache.RemoveAsync(key, ct);

    public async Task<TInstance?> EvictAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        var bytes = await DistributedCache.GetAsync(key, ct);
        if (bytes is null)
            return null;
        await DistributedCache.RemoveAsync(key, ct);
        return JsonSerializer.Deserialize<TInstance>(bytes, JsonSerializerOptions);
    }
}
