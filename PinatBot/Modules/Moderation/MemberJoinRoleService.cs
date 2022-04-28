using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class MemberJoinRoleService
{
    private readonly IDbContextFactory<Database> _dbContextFactory;
    private readonly Discord _discord;

    public MemberJoinRoleService(IDbContextFactory<Database> dbContextFactory, Discord discord)
    {
        _dbContextFactory = dbContextFactory;
        _discord = discord;
    }

    public async Task<Result> GiveMemberJoinRoleAsync(IGuildMemberAdd m, CancellationToken cancellationToken)
    {
        if (!m.User.IsDefined(out var user))
            return Result.FromError(new ArgumentInvalidError(nameof(m.User), "User is not defined"));

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var roleConfig = database.MemberJoinRoleConfigs.AsNoTracking().FirstOrDefault(role => role.GuildId == m.GuildID.Value);
        if (roleConfig is not { Enabled: true })
            return Result.FromSuccess();

        var addGuildMemberRole = await _discord.Rest.Guild.AddGuildMemberRoleAsync(m.GuildID, user.ID, new Snowflake(roleConfig.RoleId), ct: cancellationToken);
        return addGuildMemberRole.IsSuccess ? Result.FromSuccess() : Result.FromError(addGuildMemberRole.Error);
    }
}
