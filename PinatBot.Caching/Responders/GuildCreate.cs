using Microsoft.Extensions.Logging;
using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildCreate(DiscordGatewayCache cache, ILogger<GuildCreate> logger) : IResponder<IGuildCreate>
{
    public Task<Result> RespondAsync(IGuildCreate gc, CancellationToken ct = default)
    {
        if (gc.Guild.IsT1)
            return Task.FromResult(Result.FromSuccess());

        var availableGuild = gc.Guild.AsT0;
        if (cache.InternalGuilds.TryGetValue(availableGuild.ID.Value, out var guild))
        {
            if (!guild.IsUnavailable.Value)
                logger.LogWarning("Received GuildCreate for {GuildName} ({GuildId}) but it is already cached and available", availableGuild.Name, availableGuild.ID);

            guild.IsUnavailable = false;
            guild.Populate(availableGuild);
            logger.LogInformation("Guild available: {GuildName} ({GuildId})", availableGuild.Name, availableGuild.ID);
        }
        else
        {
            guild = new Guild(availableGuild.ID) { IsUnavailable = false };
            guild.Populate(availableGuild);
            cache.InternalGuilds[availableGuild.ID.Value] = guild;
            logger.LogInformation("Guild joined: {GuildName} ({GuildId})", availableGuild.Name, availableGuild.ID);
        }

        foreach (var member in availableGuild.Members)
            if (member.User.IsDefined(out var user))
                cache.InternalUsers[user.ID.Value] = user;

        return Task.FromResult(Result.FromSuccess());
    }
}
