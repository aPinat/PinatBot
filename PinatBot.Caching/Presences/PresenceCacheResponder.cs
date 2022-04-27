using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Presences;

public class PresenceCacheResponder : IResponder<IGuildCreate>, IResponder<IPresenceUpdate>
{
    public PresenceCacheResponder(PresenceCacheService presenceCache) => PresenceCache = presenceCache;
    private PresenceCacheService PresenceCache { get; }

    public Task<Result> RespondAsync(IGuildCreate g, CancellationToken ct = default)
    {
        if (!g.Presences.IsDefined(out var presences))
            return Task.FromResult(Result.FromSuccess());

        foreach (var presence in presences)
        {
            if (!presence.User.IsDefined(out var user) || !user.ID.IsDefined(out var userId))
                continue;
            PresenceCache.Set(userId, presence);
        }

        return Task.FromResult(Result.FromSuccess());
    }

    public Task<Result> RespondAsync(IPresenceUpdate p, CancellationToken ct = default)
    {
        if (!p.User.ID.IsDefined(out var userId))
            return Task.FromResult(Result.FromError(new InvalidOperationError("User ID is not defined")));

        PresenceCache.Set(userId, p);
        return Task.FromResult(Result.FromSuccess());
    }
}
