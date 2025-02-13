using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageUpdate(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider) : IResponder<IMessageUpdate>
{
    public async Task<Result> RespondAsync(IMessageUpdate m, CancellationToken ct = default)
    {
        var key = DistributedCacheProvider.CreateMessageCacheKey(m.ChannelID, m.ID);

        if (!cache.InternalMessages.TryGetValue(m.ID.Value, out var cachedMessage))
            cachedMessage = new Message(m.ID, m.ChannelID);

        cachedMessage.Update(m);
        await distributedCacheProvider.CacheAsync<IMessage>(key, cachedMessage, ct);
        return Result.FromSuccess();
    }
}
