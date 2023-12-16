using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildDelete(DiscordGatewayCache cache, ILogger<GuildDelete> logger) : IResponder<IGuildDelete>
{
    public Task<Result> RespondAsync(IGuildDelete gd, CancellationToken ct = default)
    {
        if (gd.IsUnavailable.IsDefined(out var isUnavailable) && isUnavailable)
        {
            if (cache.InternalGuilds.TryGetValue(gd.ID.Value, out var guild))
            {
                if (guild.IsUnavailable.Value)
                    logger.LogWarning("Received GuildDelete for {GuildName} ({GuildId}) but it is not available", guild.Name, guild.ID);

                guild.IsUnavailable = true;
                logger.LogWarning("Guild unavailable: {GuildName} ({GuildId})", guild.Name, guild.ID);
                return Task.FromResult(Result.FromSuccess());
            }
        }
        else
        {
            if (cache.InternalGuilds.TryRemove(gd.ID.Value, out var guild))
            {
                logger.LogInformation("Guild left: {GuildName} ({GuildId})", guild.Name, guild.ID);
                return Task.FromResult(Result.FromSuccess());
            }
        }

        logger.LogWarning("Received GuildDelete for {GuildId} but it is not cached", gd.ID);
        return Task.FromResult(Result.FromError(new InvalidOperationError("Guild is not cached.")));
    }
}
