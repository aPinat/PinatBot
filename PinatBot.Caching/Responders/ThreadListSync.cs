using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ThreadListSync : IResponder<IThreadListSync>
{
    private readonly DiscordGatewayCache _cache;

    public ThreadListSync(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IThreadListSync t, CancellationToken ct = default)
    {
        foreach (var thread in t.Threads)
            _cache.InternalGuilds[t.GuildID.Value].ThreadsInternal[thread.ID.Value] = thread;

        return Task.FromResult(Result.FromSuccess());
    }
}
