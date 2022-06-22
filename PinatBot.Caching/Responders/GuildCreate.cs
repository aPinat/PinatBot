using Microsoft.Extensions.Logging;
using PinatBot.Caching.Objects;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildCreate : IResponder<IGuildCreate>
{
    private readonly DiscordGatewayCache _cache;
    private readonly ILogger<GuildCreate> _logger;

    public GuildCreate(DiscordGatewayCache cache, ILogger<GuildCreate> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Result> RespondAsync(IGuildCreate gc, CancellationToken ct = default)
    {
        if (_cache.InternalGuilds.TryGetValue(gc.ID.Value, out var guild))
        {
            if (!guild.IsUnavailable.Value)
                _logger.LogWarning("Received GuildCreate for {GuildName} ({GuildId}) but it is already cached and available", gc.Name, gc.ID);

            guild.IsUnavailable = false;
            guild.Populate(gc);
            _logger.LogInformation("Guild available: {GuildName} ({GuildId})", gc.Name, gc.ID);
        }
        else
        {
            guild = new Guild(gc.ID) { IsUnavailable = false };
            guild.Populate(gc);
            _cache.InternalGuilds[gc.ID.Value] = guild;
            _logger.LogInformation("Guild joined: {GuildName} ({GuildId})", gc.Name, gc.ID);
        }

        if (gc.Members.IsDefined(out var members))
            foreach (var member in members)
                if (member.User.IsDefined(out var user))
                    _cache.InternalUsers[user.ID.Value] = user;

        return Task.FromResult(Result.FromSuccess());
    }
}
