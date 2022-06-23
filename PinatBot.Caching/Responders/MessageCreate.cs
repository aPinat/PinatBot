using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class MessageCreate : IResponder<IMessageCreate>
{
    private readonly DiscordGatewayCache _cache;
    private readonly DistributedCacheProvider _distributedCacheProvider;

    public MessageCreate(DiscordGatewayCache cache, DistributedCacheProvider distributedCacheProvider)
    {
        _cache = cache;
        _distributedCacheProvider = distributedCacheProvider;
    }

    public async Task<Result> RespondAsync(IMessageCreate m, CancellationToken ct = default)
    {
        var message = new Message(m.ID, m.ChannelID);
        message.Populate(m);
        _cache.InternalMessages[m.ID.Value] = message;

        var key = DistributedCacheProvider.CreateMessageCacheKey(m.ChannelID, m.ID);
        await _distributedCacheProvider.CacheAsync<IMessage>(key, message, ct);

        return Result.FromSuccess();
    }
}
