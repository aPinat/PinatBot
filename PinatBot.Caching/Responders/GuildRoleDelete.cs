using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildRoleDelete(DiscordGatewayCache cache) : IResponder<IGuildRoleDelete>
{
    public Task<Result> RespondAsync(IGuildRoleDelete r, CancellationToken ct = default)
    {
        cache.InternalGuilds[r.GuildID.Value].RolesInternal.TryRemove(r.RoleID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
