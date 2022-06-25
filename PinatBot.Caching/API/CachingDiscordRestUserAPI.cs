using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestUserAPI : IDiscordRestUserAPI, IRestCustomizable
{
    private readonly IDiscordRestUserAPI _actual;
    private readonly DiscordGatewayCache _gatewayCache;

    public CachingDiscordRestUserAPI(IDiscordRestUserAPI actual, DiscordGatewayCache gatewayCache)
    {
        _actual = actual;
        _gatewayCache = gatewayCache;
    }

    public async Task<Result<IUser>> GetCurrentUserAsync(CancellationToken ct = default)
    {
        if (_gatewayCache.CurrentUser is not null)
            return Result<IUser>.FromSuccess(_gatewayCache.CurrentUser);

        var userResult = await _actual.GetCurrentUserAsync(ct);
        if (!userResult.IsSuccess)
            return userResult;

        var user = userResult.Entity;
        _gatewayCache.CurrentUser = user;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return userResult;
    }

    public async Task<Result<IUser>> GetUserAsync(Snowflake userID, CancellationToken ct = default)
    {
        if (_gatewayCache.InternalUsers.TryGetValue(userID.Value, out var cachedUser))
            return Result<IUser>.FromSuccess(cachedUser);

        var userResult = await _actual.GetUserAsync(userID, ct);
        if (!userResult.IsSuccess)
            return userResult;

        var user = userResult.Entity;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return userResult;
    }

    public async Task<Result<IGuildMember>> GetCurrentUserGuildMemberAsync(Snowflake guildID, CancellationToken ct = default)
    {
        if (_gatewayCache.CurrentUser is not null &&
            _gatewayCache.InternalGuilds.TryGetValue(guildID.Value, out var guild) &&
            guild.MembersInternal.TryGetValue(_gatewayCache.CurrentUser.ID.Value, out var member))
            return Result<IGuildMember>.FromSuccess(member);

        var memberResult = await _actual.GetCurrentUserGuildMemberAsync(guildID, ct);
        if (!memberResult.IsSuccess)
            return memberResult;

        var guildMember = memberResult.Entity;
        if (!guildMember.User.IsDefined(out var user))
            return memberResult;
        _gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
        _gatewayCache.InternalUsers[user.ID.Value] = user;

        return memberResult;
    }
}
