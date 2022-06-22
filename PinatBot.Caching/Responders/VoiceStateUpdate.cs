using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class VoiceStateUpdate : IResponder<IVoiceStateUpdate>
{
    private readonly DiscordGatewayCache _cache;
    private readonly ILogger<VoiceStateUpdate> _logger;

    public VoiceStateUpdate(DiscordGatewayCache cache, ILogger<VoiceStateUpdate> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public Task<Result> RespondAsync(IVoiceStateUpdate vs, CancellationToken ct = default)
    {
        if (!vs.GuildID.IsDefined(out var guildId))
        {
            _logger.LogError("Received VoiceStateUpdate with undefined Guild ID");
            return Task.FromResult(Result.FromError(new InvalidOperationError("Guild ID is not defined")));
        }

        if (!_cache.InternalGuilds.TryGetValue(guildId.Value, out var guild))
        {
            _logger.LogError("Received VoiceStateUpdate for guild {GuildId} but it is not cached", guildId);
            return Task.FromResult(Result.FromError(new InvalidOperationError("Guild is not cached")));
        }

        guild.VoiceStatesInternal[vs.UserID.Value] = vs;
        return Task.FromResult(Result.FromSuccess());
    }
}
