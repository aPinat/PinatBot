using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ChannelCreate : IResponder<IChannelCreate>
{
    private readonly DiscordGatewayCache _cache;

    public ChannelCreate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IChannelCreate c, CancellationToken ct = default)
    {
        if (!c.GuildID.IsDefined(out var guildID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("GuildID is not defined")));

        _cache.InternalGuilds[guildID.Value].ChannelsInternal[c.ID.Value] = c;
        return Task.FromResult(Result.FromSuccess());
    }
}
