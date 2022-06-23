using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceDelete : IResponder<IStageInstanceDelete>
{
    private readonly DiscordGatewayCache _cache;

    public StageInstanceDelete(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IStageInstanceDelete s, CancellationToken ct = default)
    {
        _cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal.TryRemove(s.ID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
