using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageDelete(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider) : IResponder<IMessageDelete>
{
    public async Task<Result> RespondAsync(IMessageDelete m, CancellationToken ct = default)
    {
        cache.InternalMessages.TryRemove(m.ID.Value, out _);

        var key = DistributedCacheProvider.CreateMessageCacheKey(m.ChannelID, m.ID);
        await distributedCacheProvider.EvictAsync(key, ct);

        return Result.FromSuccess();
    }
}
