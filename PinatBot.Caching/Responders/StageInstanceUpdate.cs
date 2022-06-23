using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceUpdate : IResponder<IStageInstanceUpdate>
{
    private readonly DiscordGatewayCache _cache;

    public StageInstanceUpdate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IStageInstanceUpdate s, CancellationToken ct = default)
    {
        _cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal[s.ID.Value] = s;
        return Task.FromResult(Result.FromSuccess());
    }
}
