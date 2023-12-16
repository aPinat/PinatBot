using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.AutoVoiceChannels;

public class AutoVoiceChannelResponder(AutoVoiceChannelService autoVoiceChannelService) : IResponder<IVoiceStateUpdate>
{
    public Task<Result> RespondAsync(IVoiceStateUpdate vsu, CancellationToken ct = default) => autoVoiceChannelService.HandleVoiceStateUpdateAsync(vsu, ct);
}
