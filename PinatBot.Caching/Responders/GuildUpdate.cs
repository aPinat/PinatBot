using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildUpdate(DiscordGatewayCache cache, ILogger<GuildUpdate> logger) : IResponder<IGuildUpdate>
{
    public Task<Result> RespondAsync(IGuildUpdate gu, CancellationToken ct = default)
    {
        if (cache.InternalGuilds.TryGetValue(gu.ID.Value, out var guild))
        {
            if (guild.IsUnavailable.Value)
                logger.LogWarning("Received GuildUpdate for {GuildName} ({GuildId}) but it is not available", guild.Name, guild.ID);

            guild.Update(gu);
            return Task.FromResult(Result.FromSuccess());
        }

        logger.LogError("Received GuildUpdate for {GuildName} ({GuildId}) but it is not cached", gu.Name, gu.ID);
        return Task.FromResult(Result.FromError(new InvalidOperationError("Guild is not cached")));
    }
}
