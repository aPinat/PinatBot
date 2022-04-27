using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class GeneralLoggingResponder : IResponder<IMessageUpdate>, IResponder<IMessageDelete>, IResponder<IGuildMemberAdd>, IResponder<IGuildMemberRemove>
{
    public GeneralLoggingResponder(GeneralLoggingService generalLoggingService) => GeneralLoggingService = generalLoggingService;
    private GeneralLoggingService GeneralLoggingService { get; }

    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => GeneralLoggingService.LogGuildMemberAddedAsync(member, ct);

    public Task<Result> RespondAsync(IGuildMemberRemove member, CancellationToken ct = default) => GeneralLoggingService.LogGuildMemberRemovedAsync(member, ct);

    public Task<Result> RespondAsync(IMessageDelete message, CancellationToken ct = default) => GeneralLoggingService.LogMessageDeletedAsync(message, ct);

    public Task<Result> RespondAsync(IMessageUpdate message, CancellationToken ct = default) => GeneralLoggingService.LogMessageUpdatedAsync(message, ct);
}
