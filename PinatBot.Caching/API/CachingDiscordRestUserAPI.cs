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
public class CachingDiscordRestUserAPI : DiscordRestUserAPI
{
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestUserAPI(IRestHttpClient restHttpClient, IOptionsMonitor<JsonSerializerOptions> jsonOptions, ICacheProvider rateLimitCache, DiscordGatewayCache gatewayCache) :
        base(restHttpClient, jsonOptions.Get("Discord"), rateLimitCache) => _gatewayCache = gatewayCache;

    /// <inheritdoc />
    public override async Task<Result<IUser>> GetCurrentUserAsync(CancellationToken ct = default)
    {
        if (_gatewayCache.CurrentUser is not null)
            return Result<IUser>.FromSuccess(_gatewayCache.CurrentUser);

        var getUser = await base.GetCurrentUserAsync(ct);
        if (!getUser.IsSuccess)
            return getUser;

        var user = getUser.Entity;
        _gatewayCache.CurrentUser = user;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return getUser;
    }

    /// <inheritdoc />
    public override async Task<Result<IUser>> GetUserAsync(Snowflake userID, CancellationToken ct = default)
    {
        if (_gatewayCache.InternalUsers.TryGetValue(userID.Value, out var cachedUser))
            return Result<IUser>.FromSuccess(cachedUser);

        var getUser = await base.GetUserAsync(userID, ct);
        if (!getUser.IsSuccess)
            return getUser;

        var user = getUser.Entity;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return getUser;
    }

    /// <inheritdoc />
    public override async Task<Result<IGuildMember>> GetCurrentUserGuildMemberAsync(Snowflake guildID, CancellationToken ct = default)
    {
        if (_gatewayCache.CurrentUser is not null &&
            _gatewayCache.InternalGuilds.TryGetValue(guildID.Value, out var guild) &&
            guild.MembersInternal.TryGetValue(_gatewayCache.CurrentUser.ID.Value, out var member))
            return Result<IGuildMember>.FromSuccess(member);

        var result = await base.GetCurrentUserGuildMemberAsync(guildID, ct);
        if (!result.IsSuccess)
            return result;

        var guildMember = result.Entity;
        if (!guildMember.User.IsDefined(out var user))
            return result;
        _gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return result;
    }
}
