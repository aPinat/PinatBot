using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildStickersUpdate : IResponder<IGuildStickersUpdate>
{
    private readonly DiscordGatewayCache _cache;

    public GuildStickersUpdate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IGuildStickersUpdate s, CancellationToken ct = default)
    {
        var stickers = _cache.InternalGuilds[s.GuildID.Value].StickersInternal;
        stickers.Clear();
        foreach (var sticker in s.Stickers)
            stickers[sticker.ID.Value] = sticker;
        return Task.FromResult(Result.FromSuccess());
    }
}
