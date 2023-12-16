using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ThreadUpdate(DiscordGatewayCache cache) : IResponder<IThreadUpdate>
{
    public Task<Result> RespondAsync(IThreadUpdate t, CancellationToken ct = default)
    {
        if (!t.GuildID.IsDefined(out var guildID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("GuildID is not defined")));

        cache.InternalGuilds[guildID.Value].ThreadsInternal[t.ID.Value] = t;
        return Task.FromResult(Result.FromSuccess());
    }
}
