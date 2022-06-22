using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Caching;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageDelete : IResponder<IMessageDelete>
{
    private readonly DiscordGatewayCache _cache;
    private readonly DistributedCacheProvider _distributedCacheProvider;

    public MessageDelete(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider)
    {
        _cache = cache;
        _distributedCacheProvider = distributedCacheProvider;
    }

    public async Task<Result> RespondAsync(IMessageDelete m, CancellationToken ct = default)
    {
        _cache.InternalMessages.TryRemove(m.ID.Value, out _);

        var key = KeyHelpers.CreateMessageCacheKey(m.ChannelID, m.ID);
        await _distributedCacheProvider.EvictAsync(key, ct);

        return Result.FromSuccess();
    }
}
