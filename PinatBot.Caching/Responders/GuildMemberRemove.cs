using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberRemove : IResponder<IGuildMemberRemove>
{
    private readonly DiscordGatewayCache _cache;

    public GuildMemberRemove(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildMemberRemove m, CancellationToken ct = default)
    {
        _cache.InternalGuilds[m.GuildID.Value].MembersInternal.TryRemove(m.User.ID.Value, out _);
        _cache.InternalUsers[m.User.ID.Value] = m.User;

        _cache.InternalGuilds[m.GuildID.Value].MemberCount--;

        return Task.FromResult(Result.FromSuccess());
    }
}
