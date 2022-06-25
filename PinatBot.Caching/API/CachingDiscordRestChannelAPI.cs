using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestChannelAPI : IDiscordRestChannelAPI, IRestCustomizable
{
    private readonly IDiscordRestChannelAPI _actual;
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestChannelAPI(IDiscordRestChannelAPI actual, DiscordGatewayCache gatewayCache)
    {
        _actual = actual;
        _gatewayCache = gatewayCache;
    }

    public Task<Result<IChannel>> GetChannelAsync(Snowflake channelID, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetChannel(channelID);
        return cacheResult.IsSuccess ? Task.FromResult(Result<IChannel>.FromSuccess(cacheResult.Entity)) : _actual.GetChannelAsync(channelID, ct);
    }

    public async Task<Result<IMessage>> GetChannelMessageAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default)
    {
        var cacheResult = await _gatewayCache.GetMessageAsync(messageID, channelID, ct);
        if (cacheResult.IsSuccess)
            return Result<IMessage>.FromSuccess(cacheResult.Entity);

        var getMessage = await _actual.GetChannelMessageAsync(channelID, messageID, ct);
        if (!getMessage.IsSuccess)
            return getMessage;

        await _gatewayCache.CacheMessageAsync(getMessage.Entity, ct);

        return getMessage;
    }

    public async Task<Result<IReadOnlyList<IMessage>>> GetChannelMessagesAsync(Snowflake channelID,
        Optional<Snowflake> around = default,
        Optional<Snowflake> before = default,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default)
    {
        var getResult = await _actual.GetChannelMessagesAsync(channelID, around, before, after, limit, ct);
        if (!getResult.IsSuccess)
            return getResult;

        foreach (var message in getResult.Entity)
            await _gatewayCache.CacheMessageAsync(message, ct);

        return getResult;
    }
}
