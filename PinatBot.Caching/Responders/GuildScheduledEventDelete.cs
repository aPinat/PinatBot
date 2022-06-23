using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildScheduledEventDelete : IResponder<IGuildScheduledEventDelete>
{
    private readonly DiscordGatewayCache _cache;

    public GuildScheduledEventDelete(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildScheduledEventDelete e, CancellationToken ct = default)
    {
        if (e.Creator.IsDefined(out var user))
            _cache.InternalUsers[user.ID.Value] = user;

        _cache.InternalGuilds[e.GuildID.Value].GuildScheduledEventsInternal.TryRemove(e.ID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
