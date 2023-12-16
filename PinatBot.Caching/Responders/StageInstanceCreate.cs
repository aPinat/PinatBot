using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class StageInstanceCreate(DiscordGatewayCache cache) : IResponder<IStageInstanceCreate>
{
    public Task<Result> RespondAsync(IStageInstanceCreate s, CancellationToken ct = default)
    {
        cache.InternalGuilds[s.GuildID.Value].StageInstancesInternal[s.ID.Value] = s;
        return Task.FromResult(Result.FromSuccess());
    }
}
