using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestGuildAPI(IDiscordRestGuildAPI actual, DiscordGatewayCache gatewayCache) : IDiscordRestGuildAPI, IRestCustomizable
{
    public Task<Result<IGuild>> GetGuildAsync(Snowflake guildID, Optional<bool> withCounts = default, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuild(guildID);
        return cacheResult.IsSuccess ? Task.FromResult(Result<IGuild>.FromSuccess(cacheResult.Entity)) : actual.GetGuildAsync(guildID, withCounts, ct);
    }

    public Task<Result<IReadOnlyList<IChannel>>> GetGuildChannelsAsync(Snowflake guildID, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuildChannels(guildID);
        return cacheResult.IsSuccess ? Task.FromResult(Result<IReadOnlyList<IChannel>>.FromSuccess(cacheResult.Entity)) : actual.GetGuildChannelsAsync(guildID, ct);
    }

    public async Task<Result<IGuildMember>> GetGuildMemberAsync(Snowflake guildID, Snowflake userID, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuildMember(guildID, userID);
        if (cacheResult.IsSuccess)
            return Result<IGuildMember>.FromSuccess(cacheResult.Entity);

        var getResult = await actual.GetGuildMemberAsync(guildID, userID, ct);
        if (!getResult.IsSuccess)
            return getResult;

        var guildMember = getResult.Entity;
        if (!guildMember.User.IsDefined(out var user))
            return getResult;

        gatewayCache.InternalGuilds[guildID.Value].MembersInternal[userID.Value] = guildMember;
        gatewayCache.InternalUsers[user.ID.Value] = user;

        return getResult;
    }

    public async Task<Result<IReadOnlyList<IGuildMember>>> ListGuildMembersAsync(Snowflake guildID,
        Optional<int> limit = default,
        Optional<Snowflake> after = default,
        CancellationToken ct = default)
    {
        var getResult = await actual.ListGuildMembersAsync(guildID, limit, after, ct);
        if (!getResult.IsSuccess)
            return getResult;

        foreach (var guildMember in getResult.Entity)
        {
            if (!guildMember.User.IsDefined(out var user))
                continue;

            gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
            gatewayCache.InternalUsers[user.ID.Value] = user;
        }

        return getResult;
    }

    public async Task<Result<IReadOnlyList<IGuildMember>>> SearchGuildMembersAsync(Snowflake guildID, string query, Optional<int> limit = default, CancellationToken ct = default)
    {
        var result = await actual.SearchGuildMembersAsync(guildID, query, limit, ct);
        if (!result.IsSuccess)
            return result;

        foreach (var guildMember in result.Entity)
        {
            if (!guildMember.User.IsDefined(out var user))
                continue;

            gatewayCache.InternalGuilds[guildID.Value].MembersInternal[user.ID.Value] = guildMember;
            gatewayCache.InternalUsers[user.ID.Value] = user;
        }

        return result;
    }

    public Task<Result<IReadOnlyList<IRole>>> GetGuildRolesAsync(Snowflake guildID, CancellationToken ct = default)
    {
        var cacheResult = gatewayCache.GetGuildRoles(guildID);
        return cacheResult.IsSuccess ? Task.FromResult(Result<IReadOnlyList<IRole>>.FromSuccess(cacheResult.Entity)) : actual.GetGuildRolesAsync(guildID, ct);
    }
}
