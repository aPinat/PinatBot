using System.Collections.Concurrent;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.VoiceStates;

public class VoiceStateCacheService
{
    private ConcurrentDictionary<string, IVoiceState> VoiceStates { get; } = new();
    private static string GetKey(Snowflake guildId, Snowflake userId) => $"{guildId}:{userId}";

    public Result<IVoiceState> Get(Snowflake guildId, Snowflake userId)
    {
        var key = GetKey(guildId, userId);
        return !VoiceStates.ContainsKey(key)
            ? Result<IVoiceState>.FromError(new NotFoundError($"Voice state for {key} not found."))
            : Result<IVoiceState>.FromSuccess(VoiceStates[key]);
    }

    public Result AddOrUpdate(Snowflake guildId, Snowflake userId, IPartialVoiceState voiceState)
    {
        VoiceStates[GetKey(guildId, userId)] = new VoiceState(
            guildId,
            voiceState.ChannelID.HasValue ? voiceState.ChannelID.Value : default,
            userId,
            voiceState.Member,
            voiceState.SessionID.HasValue ? voiceState.SessionID.Value : string.Empty,
            voiceState.IsDeafened.HasValue && voiceState.IsDeafened.Value,
            voiceState.IsMuted.HasValue && voiceState.IsMuted.Value,
            voiceState.IsSelfDeafened.HasValue && voiceState.IsSelfDeafened.Value,
            voiceState.IsSelfMuted.HasValue && voiceState.IsSelfMuted.Value,
            voiceState.IsStreaming.HasValue && voiceState.IsStreaming.Value,
            voiceState.IsVideoEnabled.HasValue && voiceState.IsVideoEnabled.Value,
            voiceState.IsSuppressed.HasValue && voiceState.IsSuppressed.Value,
            voiceState.RequestToSpeakTimestamp.HasValue ? voiceState.RequestToSpeakTimestamp.Value : default
        );
        return Result.FromSuccess();
    }
}
