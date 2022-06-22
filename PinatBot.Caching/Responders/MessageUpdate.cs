using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageUpdate : IResponder<IMessageUpdate>
{
    private readonly DiscordGatewayCache _cache;
    private readonly DistributedCacheProvider _distributedCacheProvider;

    public MessageUpdate(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider)
    {
        _cache = cache;
        _distributedCacheProvider = distributedCacheProvider;
    }

    public async Task<Result> RespondAsync(IMessageUpdate m, CancellationToken ct = default)
    {
        if (!m.ChannelID.IsDefined(out var channelID) || !m.ID.IsDefined(out var messageID))
            return Result.FromError(new InvalidOperationError("ChannelID or MessageID is not defined"));

        var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);

        if (!_cache.InternalMessages.TryGetValue(messageID.Value, out var cachedMessage))
        {
            var cachedMessageDistributed = await _distributedCacheProvider.RetrieveAsync<IMessage>(key, ct);
            if (cachedMessageDistributed is null)
                return Result.FromError(new InvalidOperationError("Message is not cached"));
            cachedMessage = new Message(cachedMessageDistributed.ID, cachedMessageDistributed.ChannelID);
        }

        cachedMessage.Update(m);
        await _distributedCacheProvider.CacheAsync<IMessage>(key, cachedMessage, ct);
        return Result.FromSuccess();
    }
}
