using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot;

public class LogResponder : IResponder<IReady>, IResponder<IGuildCreate>
{
    private readonly ILogger<LogResponder> _logger;
    public LogResponder(ILogger<LogResponder> logger) => _logger = logger;

    public Task<Result> RespondAsync(IGuildCreate guild, CancellationToken ct = default)
    {
        _logger.LogInformation("Guild available: {GuildName} ({GuildId})", guild.Name, guild.ID);
        return Task.FromResult(Result.FromSuccess());
    }

    public Task<Result> RespondAsync(IReady ready, CancellationToken ct = default)
    {
        _logger.LogInformation("Gateway is ready");
        return Task.FromResult(Result.FromSuccess());
    }
}
