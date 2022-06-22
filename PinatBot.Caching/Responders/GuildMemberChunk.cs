using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberChunk : IResponder<IGuildMembersChunk>
{
    private readonly DiscordGatewayCache _cache;

    public GuildMemberChunk(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildMembersChunk m, CancellationToken ct = default)
    {
        foreach (var member in m.Members)
        {
            if (!member.User.IsDefined(out var user))
                continue;

            _cache.InternalGuilds[m.GuildID.Value].MembersInternal[user.ID.Value] = member;
            _cache.InternalUsers[user.ID.Value] = user;
        }

        if (!m.Presences.IsDefined(out var presences))
            return Task.FromResult(Result.FromSuccess());

        foreach (var presence in presences)
        {
            if (!presence.User.ID.IsDefined(out var userID))
                continue;

            _cache.InternalGuilds[m.GuildID.Value].PresencesInternal[userID.Value] = presence;
        }

        return Task.FromResult(Result.FromSuccess());
    }
}
