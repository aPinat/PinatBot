using Microsoft.Extensions.Logging;
using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class Ready(DiscordGatewayCache cache, ILogger<Ready> logger) : IResponder<IReady>
{
    public Task<Result> RespondAsync(IReady r, CancellationToken ct = default)
    {
        logger.LogInformation("Logged in as {DiscordTag} ({ID})", $"{r.User.Username}#{r.User.Discriminator:0000}", r.User.ID);
        logger.LogInformation("Received {GuildCount} unavailable guilds", r.Guilds.Count);

        cache.CurrentUser = r.User;
        cache.InternalUsers[r.User.ID.Value] = r.User;

        cache.InternalGuilds.Clear();
        foreach (var unavailableGuild in r.Guilds)
        {
            var guild = new Guild(unavailableGuild.ID) { IsUnavailable = true };
            cache.InternalGuilds[unavailableGuild.ID.Value] = guild;
        }

        return Task.FromResult(Result.FromSuccess());
    }
}
