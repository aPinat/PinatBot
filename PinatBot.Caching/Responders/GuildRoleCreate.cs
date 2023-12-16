using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildRoleCreate(DiscordGatewayCache cache) : IResponder<IGuildRoleCreate>
{
    public Task<Result> RespondAsync(IGuildRoleCreate r, CancellationToken ct = default)
    {
        cache.InternalGuilds[r.GuildID.Value].RolesInternal[r.Role.ID.Value] = r.Role;
        return Task.FromResult(Result.FromSuccess());
    }
}
