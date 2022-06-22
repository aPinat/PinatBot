using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildEmojisUpdate : IResponder<IGuildEmojisUpdate>
{
    private readonly DiscordGatewayCache _cache;

    public GuildEmojisUpdate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildEmojisUpdate e, CancellationToken ct = default)
    {
        var emojis = _cache.InternalGuilds[e.GuildID.Value].EmojisInternal;
        emojis.Clear();
        foreach (var emoji in e.Emojis)
        {
            if (!emoji.ID.HasValue)
                continue;
            emojis[emoji.ID.Value.Value] = emoji;
        }

        return Task.FromResult(Result.FromSuccess());
    }
}
