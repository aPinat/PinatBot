using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class VoiceStateLoggingResponder : IResponder<IVoiceStateUpdate>
{
    private readonly VoiceStateLoggingService _voiceStateLoggingService;
    public VoiceStateLoggingResponder(VoiceStateLoggingService voiceStateLoggingService) => _voiceStateLoggingService = voiceStateLoggingService;

    public Task<Result> RespondAsync(IVoiceStateUpdate vsu, CancellationToken ct = default) => _voiceStateLoggingService.LogVoiceStateUpdateAsync(vsu, ct);
}
