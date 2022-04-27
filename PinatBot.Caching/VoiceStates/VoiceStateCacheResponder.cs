using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.VoiceStates;

public class VoiceStateCacheResponder : IResponder<IGuildCreate>, IResponder<IVoiceStateUpdate>
{
    public VoiceStateCacheResponder(VoiceStateCacheService voiceStateCache) => VoiceStateCache = voiceStateCache;
    private VoiceStateCacheService VoiceStateCache { get; }

    public Task<Result> RespondAsync(IGuildCreate g, CancellationToken ct = default)
    {
        if (!g.VoiceStates.IsDefined(out var voiceStates))
            return Task.FromResult(Result.FromSuccess());

        foreach (var voiceState in voiceStates)
        {
            if (!voiceState.UserID.IsDefined(out var userId))
                continue;
            VoiceStateCache.AddOrUpdate(g.ID, userId, voiceState);
        }

        return Task.FromResult(Result.FromSuccess());
    }

    public Task<Result> RespondAsync(IVoiceStateUpdate vs, CancellationToken ct = default)
    {
        if (!vs.GuildID.IsDefined(out var guildId))
            return Task.FromResult(Result.FromError(new InvalidOperationError("Guild ID is not defined")));

        VoiceStateCache.AddOrUpdate(guildId, vs.UserID, vs);
        return Task.FromResult(Result.FromSuccess());
    }
}
