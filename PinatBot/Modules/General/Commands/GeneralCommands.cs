using System.ComponentModel;
using System.Text.Json;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.General.Commands;

public class GeneralCommands(IFeedbackService feedbackService, Discord discord, IOperationContext context) : CommandGroup
{
    [Command("echo", "say")]
    [Description("Echoes the message of the user.")]
    public async Task<IResult> SayAsync([Description("string to echo")] [Greedy] string echo)
        => await feedbackService.SendContextualNeutralAsync(echo);

    [Command("ping")]
    [Description("Get ping between bot and Discord websocket.")]
    public async Task<IResult> PingAsync()
        => await feedbackService.SendContextualSuccessAsync($"Pong!\nPing to Discord WebSocket: {discord.GatewayClient.Latency.Milliseconds}ms");

    [Command("game")]
    [Description("Get current game for user.")]
    public async Task<IResult> GameAsync([Description("Member to get game for")] IGuildMember member)
    {
        if (!context.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        if (!member.User.IsDefined(out var memberUser))
            return await feedbackService.SendContextualErrorAsync("Could not get user.");

        if (!discord.GatewayCache.GetGuildPresence(guildId, memberUser.ID).IsDefined(out var presence) ||
            !presence.Activities.IsDefined(out var activities))
            return await feedbackService.SendContextualNeutralAsync($"{member.Mention()} is not currently playing anything.");

        var activity = activities.FirstOrDefault(activity => activity.Type == ActivityType.Game);
        return activity is null
            ? await feedbackService.SendContextualNeutralAsync($"{member.Mention()} is not currently playing anything.")
            : await feedbackService.SendContextualSuccessAsync($"{member.Mention()} is currently playing {activity.Name}");
    }

    [Command("presence")]
    [Description("Get presence for user.")]
    public async Task<IResult> PresenceAsync([Description("Member to get presence for")] IGuildMember member)
    {
        if (!context.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        if (!member.User.IsDefined(out var memberUser))
            return await feedbackService.SendContextualErrorAsync("Could not get user.");

        if (!discord.GatewayCache.GetGuildPresence(guildId, memberUser.ID).IsDefined(out var presence))
            return await feedbackService.SendContextualErrorAsync($"Cannot find presence for {member.Mention()}.");

        var presenceJson = JsonSerializer.Serialize(presence, discord.JsonSerializerOptions);
        return await feedbackService.SendContextualSuccessAsync($"Presence for {member.Mention()}\n```{presenceJson}```");
    }

    [Command("user", "member", "whois")]
    [Description("Get info for a user.")]
    public async Task<IResult> UserInfoAsync([Description("User to get info for")] IUser user)
    {
        if (!context.TryGetGuildID(out var guildId))
            goto USER;

        var memberResult = await discord.Rest.Guild.GetGuildMemberAsync(guildId, user.ID);
        if (memberResult.IsDefined(out var member))
            return await feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(member, discord.JsonSerializerOptions)}```");

        USER:
        return await feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(user, discord.JsonSerializerOptions)}```");
    }
}
