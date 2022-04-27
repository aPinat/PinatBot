using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class VoiceStateLoggingResponder : IResponder<IVoiceStateUpdate>
{
    public VoiceStateLoggingResponder(VoiceStateLoggingService voiceStateLoggingService) => VoiceStateLoggingService = voiceStateLoggingService;
    private VoiceStateLoggingService VoiceStateLoggingService { get; }

    public Task<Result> RespondAsync(IVoiceStateUpdate vsu, CancellationToken ct = default) => VoiceStateLoggingService.LogVoiceStateUpdateAsync(vsu, ct);
}
