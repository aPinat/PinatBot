using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class TreeNameResolver : ITreeNameResolver
{
    public const string InteractionCommandTreeName = "interaction";
    public const string MessageCommandTreeName = "message";

    public Task<Result<(string TreeName, bool AllowDefaultTree)>> GetTreeNameAsync(ICommandContext context, CancellationToken ct = default) =>
        Task.FromResult(context switch
        {
            InteractionContext => Result<(string TreeName, bool AllowDefaultTree)>.FromSuccess((InteractionCommandTreeName, false)),
            MessageContext => Result<(string TreeName, bool AllowDefaultTree)>.FromSuccess((MessageCommandTreeName, false)),
            _ => Result<(string TreeName, bool AllowDefaultTree)>.FromError(new ArgumentOutOfRangeError(nameof(context)))
        });
}
