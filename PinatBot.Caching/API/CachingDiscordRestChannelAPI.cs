using System.Text.Json;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching.Abstractions.Services;
using Remora.Discord.Rest.API;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

/// <inheritdoc />
public class CachingDiscordRestChannelAPI : DiscordRestChannelAPI
{
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestChannelAPI(IRestHttpClient restHttpClient, IOptionsMonitor<JsonSerializerOptions> jsonOptions, ICacheProvider rateLimitCache, DiscordGatewayCache gatewayCache) :
        base(restHttpClient, jsonOptions.Get("Discord"), rateLimitCache) => _gatewayCache = gatewayCache;

    /// <inheritdoc />
    public override async Task<Result<IChannel>> GetChannelAsync(Snowflake channelID, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetChannel(channelID);
        if (cacheResult.IsSuccess)
            return Result<IChannel>.FromSuccess(cacheResult.Entity);

        var getChannel = await base.GetChannelAsync(channelID, ct);
        return getChannel;
    }

    /// <inheritdoc />
    public override async Task<Result<IMessage>> GetChannelMessageAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default)
    {
        var cacheResult = await _gatewayCache.GetMessageAsync(messageID, channelID, ct);
        if (cacheResult.IsSuccess)
            return Result<IMessage>.FromSuccess(cacheResult.Entity);

        var getMessage = await base.GetChannelMessageAsync(channelID, messageID, ct);
        if (!getMessage.IsSuccess)
            return getMessage;

        await _gatewayCache.CacheMessageAsync(getMessage.Entity, ct);

        return getMessage;
    }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<IMessage>>> GetChannelMessagesAsync(Snowflake channelID,
        Optional<Snowflake> around = default,
        Optional<Snowflake> before = default,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default)
    {
        var getResult = await base.GetChannelMessagesAsync(channelID, around, before, after, limit, ct);
        if (!getResult.IsSuccess)
            return getResult;

        foreach (var message in getResult.Entity)
            await _gatewayCache.CacheMessageAsync(message, ct);

        return getResult;
    }
}
