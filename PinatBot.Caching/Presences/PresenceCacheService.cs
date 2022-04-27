using System.Collections.Concurrent;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.Presences;

public class PresenceCacheService
{
    private ConcurrentDictionary<ulong, IPresence> Presences { get; } = new();

    public Result<IPresence> Get(Snowflake userId)
    {
        var key = userId.Value;
        return !Presences.ContainsKey(key)
            ? Result<IPresence>.FromError(new NotFoundError($"Presence for {key} not found."))
            : Result<IPresence>.FromSuccess(Presences[key]);
    }

    public Result Set(Snowflake userId, IPartialPresence presence)
    {
        Presences[userId.Value] = new Presence(
            new PartialUser(userId),
            presence.GuildID.HasValue ? presence.GuildID.Value : default,
            presence.Status.HasValue ? presence.Status.Value : ClientStatus.Offline,
            presence.Activities.HasValue ? presence.Activities.Value : null,
            presence.ClientStatus.HasValue ? presence.ClientStatus.Value : new ClientStatuses()
        );
        return Result.FromSuccess();
    }
}
