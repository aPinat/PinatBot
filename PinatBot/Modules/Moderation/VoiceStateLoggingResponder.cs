using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class VoiceStateLoggingResponder(VoiceStateLoggingService voiceStateLoggingService) : IResponder<IVoiceStateUpdate>
{
    public Task<Result> RespondAsync(IVoiceStateUpdate vsu, CancellationToken ct = default) => voiceStateLoggingService.LogVoiceStateUpdateAsync(vsu, ct);
}
