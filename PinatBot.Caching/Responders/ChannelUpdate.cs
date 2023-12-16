using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ChannelUpdate(DiscordGatewayCache cache) : IResponder<IChannelUpdate>
{
    public Task<Result> RespondAsync(IChannelUpdate c, CancellationToken ct = default)
    {
        if (!c.GuildID.IsDefined(out var guildID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("GuildID is not defined")));

        cache.InternalGuilds[guildID.Value].ChannelsInternal[c.ID.Value] = c;
        return Task.FromResult(Result.FromSuccess());
    }
}
