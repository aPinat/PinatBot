using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ThreadListSync(DiscordGatewayCache cache) : IResponder<IThreadListSync>
{
    public Task<Result> RespondAsync(IThreadListSync t, CancellationToken ct = default)
    {
        foreach (var thread in t.Threads)
            cache.InternalGuilds[t.GuildID.Value].ThreadsInternal[thread.ID.Value] = thread;

        return Task.FromResult(Result.FromSuccess());
    }
}
