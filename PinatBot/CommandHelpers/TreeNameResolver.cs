using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class TreeNameResolver : ITreeNameResolver
{
    public const string InteractionCommandTreeName = "interaction";
    public const string MessageCommandTreeName = "message";

    public Task<Result<string>> GetTreeNameAsync(IOperationContext context, CancellationToken ct = default) =>
        Task.FromResult(context switch
        {
            IInteractionContext => InteractionCommandTreeName,
            IMessageContext => MessageCommandTreeName,
            _ => Result<string>.FromError(new ArgumentOutOfRangeError(nameof(context)))
        });
}
