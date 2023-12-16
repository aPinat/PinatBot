using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Remora.Rest.Core;

namespace PinatBot.Caching;

public class DistributedCacheProvider(IDistributedCache distributedCache, IOptionsMonitor<JsonSerializerOptions> jsonOptions)
{
    private JsonSerializerOptions JsonSerializerOptions { get; } = jsonOptions.Get("Discord");

    public Task CacheAsync<TInstance>(string key, TInstance instance, CancellationToken ct = default)
        where TInstance : class
    {
        var serialized = JsonSerializer.Serialize(instance, JsonSerializerOptions);
        return distributedCache.SetStringAsync(key, serialized, ct);
    }

    public async Task<TInstance?> RetrieveAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        var value = await distributedCache.GetAsync(key, ct);
        return value is null ? null : JsonSerializer.Deserialize<TInstance>(value, JsonSerializerOptions);
    }

    public Task EvictAsync(string key, CancellationToken ct = default) => distributedCache.RemoveAsync(key, ct);

    public async Task<TInstance?> EvictAsync<TInstance>(string key, CancellationToken ct = default) where TInstance : class
    {
        var bytes = await distributedCache.GetAsync(key, ct);
        if (bytes is null)
            return null;
        await distributedCache.RemoveAsync(key, ct);
        return JsonSerializer.Deserialize<TInstance>(bytes, JsonSerializerOptions);
    }

    public static string CreateChannelCacheKey(in Snowflake channelID) => $"Channel:{channelID}";

    public static string CreateMessageCacheKey(in Snowflake channelID, in Snowflake messageID) => $"{CreateChannelCacheKey(channelID)}:Message:{messageID}";
}
