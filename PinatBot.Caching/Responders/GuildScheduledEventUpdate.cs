using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildScheduledEventUpdate : IResponder<IGuildScheduledEventUpdate>
{
    private readonly DiscordGatewayCache _cache;

    public GuildScheduledEventUpdate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildScheduledEventUpdate e, CancellationToken ct = default)
    {
        if (e.Creator.IsDefined(out var user))
            _cache.InternalUsers[user.ID.Value] = user;

        _cache.InternalGuilds[e.GuildID.Value].GuildScheduledEventsInternal[e.ID.Value] = e;
        return Task.FromResult(Result.FromSuccess());
    }
}
