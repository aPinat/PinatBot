using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestStickerAPI : IDiscordRestStickerAPI, IRestCustomizable
{
    private readonly IDiscordRestStickerAPI _actual;
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestStickerAPI(IDiscordRestStickerAPI actual, DiscordGatewayCache gatewayCache)
    {
        _actual = actual;
        _gatewayCache = gatewayCache;
    }

    public async Task<Result<ISticker>> GetGuildStickerAsync(Snowflake guildId, Snowflake stickerId, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuildSticker(guildId, stickerId);
        if (cacheResult.IsSuccess)
            return Result<ISticker>.FromSuccess(cacheResult.Entity);

        var getResult = await _actual.GetGuildStickerAsync(guildId, stickerId, ct);
        if (!getResult.IsSuccess)
            return getResult;

        _gatewayCache.InternalGuilds[guildId.Value].StickersInternal[stickerId.Value] = getResult.Entity;

        return getResult;
    }

    public async Task<Result<IReadOnlyList<ISticker>>> ListGuildStickersAsync(Snowflake guildId, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuildStickers(guildId);
        if (cacheResult.IsSuccess)
            return Result<IReadOnlyList<ISticker>>.FromSuccess(cacheResult.Entity);

        var result = await _actual.ListGuildStickersAsync(guildId, ct);
        if (!result.IsSuccess)
            return result;

        var stickers = _gatewayCache.InternalGuilds[guildId.Value].StickersInternal;
        stickers.Clear();
        foreach (var sticker in result.Entity)
            stickers[sticker.ID.Value] = sticker;

        return result;
    }
}
