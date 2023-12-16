using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceDelete(DiscordGatewayCache cache) : IResponder<IStageInstanceDelete>
{
    public Task<Result> RespondAsync(IStageInstanceDelete s, CancellationToken ct = default)
    {
        cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal.TryRemove(s.ID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
