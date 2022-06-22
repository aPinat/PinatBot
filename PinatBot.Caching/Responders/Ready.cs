using Microsoft.Extensions.Logging;
using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class Ready : IResponder<IReady>
{
    private readonly DiscordGatewayCache _cache;
    private readonly ILogger<Ready> _logger;

    public Ready(DiscordGatewayCache cache, ILogger<Ready> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Result> RespondAsync(IReady r, CancellationToken ct = default)
    {
        _logger.LogInformation("Logged in as {DiscordTag} ({ID})", $"{r.User.Username}#{r.User.Discriminator:0000}", r.User.ID);
        _logger.LogInformation("Received {GuildCount} unavailable guilds", r.Guilds.Count);

        _cache.CurrentUser = r.User;
        _cache.InternalUsers[r.User.ID.Value] = r.User;

        _cache.InternalGuilds.Clear();
        foreach (var unavailableGuild in r.Guilds)
        {
            var guild = new Guild(unavailableGuild.ID) { IsUnavailable = true };
            _cache.InternalGuilds[unavailableGuild.ID.Value] = guild;
        }

        return Task.FromResult(Result.FromSuccess());
    }
}
