using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildRoleUpdate(DiscordGatewayCache cache) : IResponder<IGuildRoleUpdate>
{
    public Task<Result> RespondAsync(IGuildRoleUpdate r, CancellationToken ct = default)
    {
        cache.InternalGuilds[r.GuildID.Value].RolesInternal[r.Role.ID.Value] = r.Role;
        return Task.FromResult(Result.FromSuccess());
    }
}
