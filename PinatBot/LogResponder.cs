using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot;

public class LogResponder : IResponder<IReady>, IResponder<IGuildCreate>
{
    public LogResponder(ILogger<LogResponder> logger) => Logger = logger;
    private ILogger<LogResponder> Logger { get; }

    public Task<Result> RespondAsync(IGuildCreate guild, CancellationToken ct = default)
    {
        Logger.LogInformation("Guild available: {GuildName} ({GuildId})", guild.Name, guild.ID);
        return Task.FromResult(Result.FromSuccess());
    }

    public Task<Result> RespondAsync(IReady ready, CancellationToken ct = default)
    {
        Logger.LogInformation("Gateway is ready");
        return Task.FromResult(Result.FromSuccess());
    }
}
