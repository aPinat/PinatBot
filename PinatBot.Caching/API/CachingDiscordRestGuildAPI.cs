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
public class CachingDiscordRestGuildAPI : DiscordRestGuildAPI
{
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestGuildAPI(IRestHttpClient restHttpClient, IOptionsMonitor<JsonSerializerOptions> jsonOptions, ICacheProvider rateLimitCache, DiscordGatewayCache gatewayCache) :
        base(restHttpClient, jsonOptions.Get("Discord"), rateLimitCache) => _gatewayCache = gatewayCache;

    /// <inheritdoc />
    public override async Task<Result<IGuild>> GetGuildAsync(Snowflake guildID, Optional<bool> withCounts = default, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuild(guildID);
        if (cacheResult.IsSuccess)
            return Result<IGuild>.FromSuccess(cacheResult.Entity);

        return await base.GetGuildAsync(guildID, withCounts, ct);
    }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<IChannel>>> GetGuildChannelsAsync(Snowflake guildID, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuildChannels(guildID);
        if (cacheResult.IsSuccess)
            return Result<IReadOnlyList<IChannel>>.FromSuccess(cacheResult.Entity);

        return await base.GetGuildChannelsAsync(guildID, ct);
    }

    /// <inheritdoc />
    public override async Task<Result<IGuildMember>> GetGuildMemberAsync(Snowflake guildID, Snowflake userID, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuildMember(guildID, userID);
        if (cacheResult.IsSuccess)
            return Result<IGuildMember>.FromSuccess(cacheResult.Entity);

        var getResult = await base.GetGuildMemberAsync(guildID, userID, ct);
        if (!getResult.IsSuccess)
            return getResult;

        var guildMember = getResult.Entity;
        if (!guildMember.User.IsDefined(out var user))
            return getResult;

        _gatewayCache.InternalGuilds[guildID.Value].MembersInternal[userID.Value] = guildMember;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return getResult;
    }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<IGuildMember>>> ListGuildMembersAsync(Snowflake guildID,
        Optional<int> limit = default,
        Optional<Snowflake> after = default,
        CancellationToken ct = default)
    {
        var getResult = await base.ListGuildMembersAsync(guildID, limit, after, ct);
        if (!getResult.IsSuccess)
            return getResult;

        foreach (var guildMember in getResult.Entity)
        {
            if (!guildMember.User.IsDefined(out var user))
                continue;

            _gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
            _gatewayCache.InternalUsers[user.ID.Value] = user;
        }

        return getResult;
    }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<IGuildMember>>> SearchGuildMembersAsync(Snowflake guildID, string query, Optional<int> limit = default, CancellationToken ct = default)
    {
        var result = await base.SearchGuildMembersAsync(guildID, query, limit, ct);
        if (!result.IsSuccess)
            return result;

        foreach (var guildMember in result.Entity)
        {
            if (!guildMember.User.IsDefined(out var user))
                continue;

            _gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
            _gatewayCache.InternalUsers[user.ID.Value] = user;
        }

        return result;
    }

    /// <inheritdoc />
    public override async Task<Result<IReadOnlyList<IRole>>> GetGuildRolesAsync(Snowflake guildID, CancellationToken ct = default)
    {
        var cacheResult = _gatewayCache.GetGuildRoles(guildID);
        if (cacheResult.IsSuccess)
            return Result<IReadOnlyList<IRole>>.FromSuccess(cacheResult.Entity);

        return await base.GetGuildRolesAsync(guildID, ct);
    }
}
