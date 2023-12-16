using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class GeneralLoggingResponder(GeneralLoggingService generalLoggingService) : IResponder<IMessageUpdate>, IResponder<IMessageDelete>, IResponder<IGuildMemberAdd>, IResponder<IGuildMemberRemove>
{
    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => generalLoggingService.LogGuildMemberAddedAsync(member, ct);

    public Task<Result> RespondAsync(IGuildMemberRemove member, CancellationToken ct = default) => generalLoggingService.LogGuildMemberRemovedAsync(member, ct);

    public Task<Result> RespondAsync(IMessageDelete message, CancellationToken ct = default) => generalLoggingService.LogMessageDeletedAsync(message, ct);

    public Task<Result> RespondAsync(IMessageUpdate message, CancellationToken ct = default) => generalLoggingService.LogMessageUpdatedAsync(message, ct);
}
