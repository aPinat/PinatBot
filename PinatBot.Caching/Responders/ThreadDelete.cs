using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class ThreadDelete : IResponder<IThreadDelete>
{
    private readonly DiscordGatewayCache _cache;

    public ThreadDelete(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IThreadDelete t, CancellationToken ct = default)
    {
        if (!t.GuildID.IsDefined(out var guildID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("GuildID is not defined")));

        if (!t.ID.IsDefined(out var threadID))
            return Task.FromResult(Result.FromError(new InvalidOperationError("ThreadID is not defined")));

        _cache.InternalGuilds[guildID.Value].ThreadsInternal.TryRemove(threadID.Value, out _);
        return Task.FromResult(Result.FromSuccess());
    }
}
