using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildScheduledEventCreate(DiscordGatewayCache cache) : IResponder<IGuildScheduledEventCreate>
{
    public Task<Result> RespondAsync(IGuildScheduledEventCreate e, CancellationToken ct = default)
    {
        if (e.Creator.IsDefined(out var user))
            cache.InternalUsers[user.ID.Value] = user;

        cache.InternalGuilds[e.GuildID.Value].GuildScheduledEventsInternal[e.ID.Value] = e;
        return Task.FromResult(Result.FromSuccess());
    }
}
