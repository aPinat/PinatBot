using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberChunk(DiscordGatewayCache cache) : IResponder<IGuildMembersChunk>
{
    public Task<Result> RespondAsync(IGuildMembersChunk m, CancellationToken ct = default)
    {
        foreach (var member in m.Members)
        {
            if (!member.User.IsDefined(out var user))
                continue;

            cache.InternalGuilds[m.GuildID.Value].MembersInternal[user.ID.Value] = member;
            cache.InternalUsers[user.ID.Value] = user;
        }

        if (!m.Presences.IsDefined(out var presences))
            return Task.FromResult(Result.FromSuccess());

        foreach (var presence in presences)
        {
            if (!presence.User.ID.IsDefined(out var userID))
                continue;

            cache.InternalGuilds[m.GuildID.Value].PresencesInternal[userID.Value] = presence;
        }

        return Task.FromResult(Result.FromSuccess());
    }
}
