using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberRemove(DiscordGatewayCache cache) : IResponder<IGuildMemberRemove>
{
    public Task<Result> RespondAsync(IGuildMemberRemove m, CancellationToken ct = default)
    {
        cache.InternalGuilds[m.GuildID.Value].MembersInternal.TryRemove(m.User.ID.Value, out _);
        cache.InternalUsers[m.User.ID.Value] = m.User;

        cache.InternalGuilds[m.GuildID.Value].MemberCount--;

        return Task.FromResult(Result.FromSuccess());
    }
}
