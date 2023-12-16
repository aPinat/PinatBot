using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestUserAPI(IDiscordRestUserAPI actual, DiscordGatewayCache gatewayCache) : IDiscordRestUserAPI, IRestCustomizable
{
    public async Task<Result<IUser>> GetCurrentUserAsync(CancellationToken ct = default)
    {
        if (gatewayCache.CurrentUser is not null)
            return Result<IUser>.FromSuccess(gatewayCache.CurrentUser);

        var userResult = await actual.GetCurrentUserAsync(ct);
        if (!userResult.IsSuccess)
            return userResult;

        var user = userResult.Entity;
        gatewayCache.CurrentUser = user;
        gatewayCache.InternalUsers[user.ID.Value] = user;

        return userResult;
    }

    public async Task<Result<IUser>> GetUserAsync(Snowflake userID, CancellationToken ct = default)
    {
        if (gatewayCache.InternalUsers.TryGetValue(userID.Value, out var cachedUser))
            return Result<IUser>.FromSuccess(cachedUser);

        var userResult = await actual.GetUserAsync(userID, ct);
        if (!userResult.IsSuccess)
            return userResult;

        var user = userResult.Entity;
        gatewayCache.InternalUsers[user.ID.Value] = user;

        return userResult;
    }

    public async Task<Result<IGuildMember>> GetCurrentUserGuildMemberAsync(Snowflake guildID, CancellationToken ct = default)
    {
        if (gatewayCache.CurrentUser is not null &&
            gatewayCache.InternalGuilds.TryGetValue(guildID.Value, out var guild) &&
            guild.MembersInternal.TryGetValue(gatewayCache.CurrentUser.ID.Value, out var member))
            return Result<IGuildMember>.FromSuccess(member);

        var memberResult = await actual.GetCurrentUserGuildMemberAsync(guildID, ct);
        if (!memberResult.IsSuccess)
            return memberResult;

        var guildMember = memberResult.Entity;
        if (!guildMember.User.IsDefined(out var user))
            return memberResult;
        gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
        gatewayCache.InternalUsers[user.ID.Value] = user;

        return memberResult;
    }
}
