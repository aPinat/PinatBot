using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberAdd : IResponder<IGuildMemberAdd>
{
    private readonly DiscordGatewayCache _cache;

    public GuildMemberAdd(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildMemberAdd m, CancellationToken ct = default)
    {
        if (!m.User.IsDefined(out var user))
            return Task.FromResult(Result.FromError(new InvalidOperationError("User is not defined")));

        _cache.InternalGuilds[m.GuildID.Value].MembersInternal[user.ID.Value] = m;
        _cache.InternalUsers[user.ID.Value] = user;

        _cache.InternalGuilds[m.GuildID.Value].MemberCount++;

        return Task.FromResult(Result.FromSuccess());
    }
}
