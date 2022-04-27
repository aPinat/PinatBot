using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching.Abstractions.Services;
using Remora.Results;

namespace PinatBot.Caching;

public class MixedCacheProvider : ICacheProvider
{
    public MixedCacheProvider(IMemoryCache memoryCache, IDistributedCache distributedCache, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
    {
        MemoryCache = memoryCache;
        DistributedCache = distributedCache;
        JsonSerializerOptions = jsonOptions.Get("Discord");
    }

    private IMemoryCache MemoryCache { get; }
    private IDistributedCache DistributedCache { get; }
    private JsonSerializerOptions JsonSerializerOptions { get; }

    public async ValueTask CacheAsync<TInstance>(string key, TInstance instance, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default)
        where TInstance : class
    {
        var memoryOptions = new MemoryCacheEntryOptions { AbsoluteExpiration = absoluteExpiration, SlidingExpiration = slidingExpiration };
        MemoryCache.Set(key, instance, memoryOptions);

        if (instance is IMessage && !key.StartsWith("Webhook"))
        {
            var distributedOptions = new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration, SlidingExpiration = slidingExpiration };
            var serialized = JsonSerializer.Serialize(instance, JsonSerializerOptions);
            await DistributedCache.SetStringAsync(key, serialized, distributedOptions, ct);
        }
    }

    public async ValueTask<Result<TInstance>> RetrieveAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        if (MemoryCache.TryGetValue<TInstance>(key, out var instance))
            return instance;

        if (!key.Contains("Message") || key.StartsWith("Webhook"))
            goto ERROR;

        var value = await DistributedCache.GetAsync(key, ct);
        if (value is null)
            goto ERROR;

        await DistributedCache.RefreshAsync(key, ct);
        return JsonSerializer.Deserialize<TInstance>(value, JsonSerializerOptions);

        ERROR:
        return new NotFoundError($"The key \"{key}\" did not contain a value in cache.");
    }

    public async ValueTask<Result> EvictAsync(string key, CancellationToken ct = default)
    {
        if (!MemoryCache.TryGetValue(key, out _))
            goto ERROR;

        MemoryCache.Remove(key);

        if (!key.Contains("Message") || key.StartsWith("Webhook"))
            goto SUCCESS;

        var value = await DistributedCache.GetAsync(key, ct);
        if (value is null)
            goto ERROR;
        await DistributedCache.RemoveAsync(key, ct);

        SUCCESS:
        return Result.FromSuccess();

        ERROR:
        return new NotFoundError($"The key \"{key}\" did not contain a value in cache.");
    }

    public async ValueTask<Result<TInstance>> EvictAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        if (!MemoryCache.TryGetValue(key, out TInstance value))
            goto ERROR;

        MemoryCache.Remove(key);

        if (!key.Contains("Message") || key.StartsWith("Webhook"))
            goto SUCCESS;

        var bytes = await DistributedCache.GetAsync(key, ct);
        if (bytes is null)
            goto ERROR;
        await DistributedCache.RemoveAsync(key, ct);
        return JsonSerializer.Deserialize<TInstance>(bytes, JsonSerializerOptions);

        SUCCESS:
        return value;

        ERROR:
        return new NotFoundError($"The key \"{key}\" did not contain a value in cache.");
    }
}
