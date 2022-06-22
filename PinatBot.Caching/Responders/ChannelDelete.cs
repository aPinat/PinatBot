using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ChannelDelete : IResponder<IChannelDelete>
{
    private readonly DiscordGatewayCache _cache;

    public ChannelDelete(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IChannelDelete c, CancellationToken ct = default)
    {
        if (!c.GuildID.IsDefined(out var guildID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("GuildID is not defined")));

        _cache.InternalGuilds[guildID.Value].ChannelsInternal.TryRemove(c.ID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
