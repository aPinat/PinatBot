using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class PresenceUpdate : IResponder<IPresenceUpdate>
{
    private readonly DiscordGatewayCache _cache;
    private readonly ILogger<PresenceUpdate> _logger;

    public PresenceUpdate(DiscordGatewayCache cache, ILogger<PresenceUpdate> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Result> RespondAsync(IPresenceUpdate p, CancellationToken ct = default)
    {
        if (!p.User.ID.IsDefined(out var userId))
        {
            _logger.LogError("Received PresenceUpdate with undefined User ID");
            return Task.FromResult(Result.FromError(new InvalidOperationError("User ID is not defined")));
        }

        if (!_cache.InternalGuilds.TryGetValue(p.GuildID.Value, out var guild))
        {
            _logger.LogError("Received PresenceUpdate for guild {GuildId} but it is not cached", p.GuildID);
            return Task.FromResult(Result.FromError(new InvalidOperationError("Guild is not cached")));
        }

        guild.Update(p, userId);
        return Task.FromResult(Result.FromSuccess());
    }
}
