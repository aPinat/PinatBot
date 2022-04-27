using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class MemberJoinRoleService
{
    public MemberJoinRoleService(IDbContextFactory<Database> dbContextFactory, IDiscordRestGuildAPI guildApi)
    {
        DbContextFactory = dbContextFactory;
        GuildApi = guildApi;
    }

    private IDbContextFactory<Database> DbContextFactory { get; }
    private IDiscordRestGuildAPI GuildApi { get; }

    public async Task<Result> GiveMemberJoinRoleAsync(IGuildMemberAdd m, CancellationToken cancellationToken)
    {
        if (!m.User.IsDefined(out var user))
            return Result.FromError(new ArgumentInvalidError(nameof(m.User), "User is not defined"));

        await using var database = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var roleConfig = database.MemberJoinRoleConfigs.AsNoTracking().FirstOrDefault(role => role.GuildId == m.GuildID.Value);
        if (roleConfig is not { Enabled: true })
            return Result.FromSuccess();

        var addGuildMemberRole = await GuildApi.AddGuildMemberRoleAsync(m.GuildID, user.ID, new Snowflake(roleConfig.RoleId), ct: cancellationToken);
        return addGuildMemberRole.IsSuccess ? Result.FromSuccess() : Result.FromError(addGuildMemberRole.Error);
    }
}
