using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageCreate(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider) : IResponder<IMessageCreate>
{
    public async Task<Result> RespondAsync(IMessageCreate m, CancellationToken ct = default)
    {
        var message = new Message(m.ID, m.ChannelID);
        message.Populate(m);
        cache.InternalMessages[m.ID.Value] = message;

        var key = DistributedCacheProvider.CreateMessageCacheKey(m.ChannelID, m.ID);
        await distributedCacheProvider.CacheAsync<IMessage>(key, message, ct);

        return Result.FromSuccess();
    }
}
