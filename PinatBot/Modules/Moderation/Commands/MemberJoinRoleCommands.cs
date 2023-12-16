using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using PinatBot.Data.Modules.Moderation;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation.Commands;

[Group("joinrole")]
[DiscordDefaultMemberPermissions(DiscordPermission.ManageRoles)]
[DiscordDefaultDMPermission(false)]
[RequireDiscordPermission(DiscordPermission.ManageRoles)]
[RequireBotDiscordPermissions(DiscordPermission.ManageRoles)]
public class MemberJoinRoleCommands(IDbContextFactory<Database> dbContextFactory, IOperationContext commandContext, IFeedbackService feedbackService)
    : CommandGroup
{
    [Command("show", "get")]
    [Description("Assign role on member join.")]
    public async Task<IResult> GetJoinRoleAsync()
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var joinRole = await database.MemberJoinRoleConfigs.AsNoTracking().FirstOrDefaultAsync(joinRole => joinRole.GuildId == guildId.Value);
        if (joinRole is null)
            return await feedbackService.SendContextualInfoAsync("No role to assign on member join is set.");

        if (joinRole.Enabled)
            return await feedbackService.SendContextualInfoAsync($"Role assigned on member join set to {new Snowflake(joinRole.RoleId).Mention(typeof(IRole))}.");

        return await feedbackService.SendContextualInfoAsync("Role assigning on member join is disabled.");
    }

    [Command("set")]
    [Description("Set role to assign on member join.")]
    public async Task<IResult> SetJoinRoleAsync([Description("Role to assign")] IRole role)
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var joinRole = await database.MemberJoinRoleConfigs.FirstOrDefaultAsync(joinRole => joinRole.GuildId == guildId.Value);
        if (joinRole is null)
        {
            joinRole = new MemberJoinRoleConfig(guildId.Value) { RoleId = role.ID.Value, Enabled = true };
            await database.MemberJoinRoleConfigs.AddAsync(joinRole);
        }
        else
        {
            joinRole.RoleId = role.ID.Value;
            joinRole.Enabled = true;
        }

        await database.SaveChangesAsync();
        return await feedbackService.SendContextualSuccessAsync($"Role assigned on member join set to {role.Mention()}.");
    }

    [Command("disable", "off", "none")]
    [Description("Disable assigning role on member join.")]
    public async Task<IResult> DisableJoinRoleAsync()
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var joinRole = await database.MemberJoinRoleConfigs.FirstOrDefaultAsync(joinRole => joinRole.GuildId == guildId.Value);
        if (joinRole is null)
            return await feedbackService.SendContextualErrorAsync("No role to assign on member join set.");

        joinRole.Enabled = false;
        await database.SaveChangesAsync();
        return await feedbackService.SendContextualSuccessAsync("Disabled assigning role on member join.");
    }
}
