using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildDelete : IResponder<IGuildDelete>
{
    private readonly DiscordGatewayCache _cache;
    private readonly ILogger<GuildDelete> _logger;

    public GuildDelete(DiscordGatewayCache cache, ILogger<GuildDelete> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Result> RespondAsync(IGuildDelete gd, CancellationToken ct = default)
    {
        if (gd.IsUnavailable.IsDefined(out var isUnavailable) && isUnavailable)
        {
            if (_cache.InternalGuilds.TryGetValue(gd.ID.Value, out var guild))
            {
                if (guild.IsUnavailable.Value)
                    _logger.LogWarning("Received GuildDelete for {GuildName} ({GuildId}) but it is not available", guild.Name, guild.ID);

                guild.IsUnavailable = true;
                _logger.LogWarning("Guild unavailable: {GuildName} ({GuildId})", guild.Name, guild.ID);
                return Task.FromResult(Result.FromSuccess());
            }
        }
        else
        {
            if (_cache.InternalGuilds.TryRemove(gd.ID.Value, out var guild))
            {
                _logger.LogInformation("Guild left: {GuildName} ({GuildId})", guild.Name, guild.ID);
                return Task.FromResult(Result.FromSuccess());
            }
        }

        _logger.LogWarning("Received GuildDelete for {GuildId} but it is not cached", gd.ID);
        return Task.FromResult(Result.FromError(new InvalidOperationError("Guild is not cached.")));
    }
}
