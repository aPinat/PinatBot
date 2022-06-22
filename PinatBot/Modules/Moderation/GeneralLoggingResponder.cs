using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class GeneralLoggingResponder : IResponder<IMessageUpdate>, IResponder<IMessageDelete>, IResponder<IGuildMemberAdd>, IResponder<IGuildMemberRemove>
{
    private readonly GeneralLoggingService _generalLoggingService;
    public GeneralLoggingResponder(GeneralLoggingService generalLoggingService) => _generalLoggingService = generalLoggingService;

    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => _generalLoggingService.LogGuildMemberAddedAsync(member, ct);

    public Task<Result> RespondAsync(IGuildMemberRemove member, CancellationToken ct = default) => _generalLoggingService.LogGuildMemberRemovedAsync(member, ct);

    public Task<Result> RespondAsync(IMessageDelete message, CancellationToken ct = default) => _generalLoggingService.LogMessageDeletedAsync(message, ct);

    public Task<Result> RespondAsync(IMessageUpdate message, CancellationToken ct = default) => _generalLoggingService.LogMessageUpdatedAsync(message, ct);
}
