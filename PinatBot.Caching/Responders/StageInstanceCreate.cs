using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceCreate : IResponder<IStageInstanceCreate>
{
    private readonly DiscordGatewayCache _cache;

    public StageInstanceCreate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IStageInstanceCreate s, CancellationToken ct = default)
    {
        _cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal[s.ID.Value] = s;
        return Task.FromResult(Result.FromSuccess());
    }
}
