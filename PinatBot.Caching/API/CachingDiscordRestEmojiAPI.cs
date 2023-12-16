using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestEmojiAPI(IDiscordRestEmojiAPI actual, DiscordGatewayCache gatewayCache) : IDiscordRestEmojiAPI, IRestCustomizable
{
    public async Task<Result<IEmoji>> GetGuildEmojiAsync(Snowflake guildID, Snowflake emojiID, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuildEmoji(guildID, emojiID);
        if (cacheResult.IsSuccess)
            return Result<IEmoji>.FromSuccess(cacheResult.Entity);

        var getResult = await actual.GetGuildEmojiAsync(guildID, emojiID, ct);
        if (!getResult.IsSuccess)
            return getResult;

        gatewayCache.InternalGuilds[guildID.Value].EmojisInternal[emojiID.Value] = getResult.Entity;

        return getResult;
    }

    public async Task<Result<IReadOnlyList<IEmoji>>> ListGuildEmojisAsync(Snowflake guildID, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuildEmojis(guildID);
        if (cacheResult.IsSuccess)
            return Result<IReadOnlyList<IEmoji>>.FromSuccess(cacheResult.Entity);

        var result = await actual.ListGuildEmojisAsync(guildID, ct);
        if (!result.IsSuccess)
            return result;

        var emojis = gatewayCache.InternalGuilds[guildID.Value].EmojisInternal;
        emojis.Clear();
        foreach (var emoji in result.Entity)
        {
            if (!emoji.ID.HasValue)
                continue;
            emojis[emoji.ID.Value.Value] = emoji;
        }

        return result;
    }
}
