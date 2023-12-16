using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceUpdate(DiscordGatewayCache cache) : IResponder<IStageInstanceUpdate>
{
    public Task<Result> RespondAsync(IStageInstanceUpdate s, CancellationToken ct = default)
    {
        cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal[s.ID.Value] = s;
        return Task.FromResult(Result.FromSuccess());
    }
}
