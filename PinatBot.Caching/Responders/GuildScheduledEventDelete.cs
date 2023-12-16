using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildScheduledEventDelete(DiscordGatewayCache cache) : IResponder<IGuildScheduledEventDelete>
{
    public Task<Result> RespondAsync(IGuildScheduledEventDelete e, CancellationToken ct = default)
    {
        if (e.Creator.IsDefined(out var user))
            cache.InternalUsers[user.ID.Value] = user;

        cache.InternalGuilds[e.GuildID.Value].GuildScheduledEventsInternal.TryRemove(e.ID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
