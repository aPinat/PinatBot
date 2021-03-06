using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildRoleCreate : IResponder<IGuildRoleCreate>
{
    private readonly DiscordGatewayCache _cache;

    public GuildRoleCreate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildRoleCreate r, CancellationToken ct = default)
    {
        _cache.InternalGuilds[r.GuildID.Value].RolesInternal[r.Role.ID.Value] = r.Role;
        return Task.FromResult(Result.FromSuccess());
    }
}
