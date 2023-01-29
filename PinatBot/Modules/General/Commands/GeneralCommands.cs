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

public class GeneralCommands : CommandGroup
{
    private readonly ICommandContext _context;
    private readonly Discord _discord;
    private readonly FeedbackService _feedbackService;

    public GeneralCommands(FeedbackService feedbackService, Discord discord, ICommandContext context)
    {
        _feedbackService = feedbackService;
        _discord = discord;
        _context = context;
    }

    [Command("echo", "say")]
    [Description("Echoes the message of the user.")]
    public async Task<IResult> SayAsync([Description("string to echo")] [Greedy] string echo)
        => await _feedbackService.SendContextualNeutralAsync(echo);

    [Command("ping")]
    [Description("Get ping between bot and Discord websocket.")]
    public async Task<IResult> PingAsync()
        => await _feedbackService.SendContextualSuccessAsync($"Pong!\nPing to Discord WebSocket: {_discord.GatewayClient.Latency.Milliseconds}ms");

    [Command("game")]
    [Description("Get current game for user.")]
    public async Task<IResult> GameAsync([Description("Member to get game for")] IGuildMember member)
    {
        if (!_context.TryGetGuildID(out var guildId))
            return await _feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        if (!member.User.IsDefined(out var memberUser))
            return await _feedbackService.SendContextualErrorAsync("Could not get user.");

        if (!_discord.GatewayCache.GetGuildPresence(guildId.Value, memberUser.ID).IsDefined(out var presence) ||
            !presence.Activities.IsDefined(out var activities))
            return await _feedbackService.SendContextualNeutralAsync($"{member.Mention()} is not currently playing anything.");

        var activity = activities.FirstOrDefault(activity => activity.Type == ActivityType.Game);
        return activity is null
            ? await _feedbackService.SendContextualNeutralAsync($"{member.Mention()} is not currently playing anything.")
            : await _feedbackService.SendContextualSuccessAsync($"{member.Mention()} is currently playing {activity.Name}");
    }

    [Command("presence")]
    [Description("Get presence for user.")]
    public async Task<IResult> PresenceAsync([Description("Member to get presence for")] IGuildMember member)
    {
        if (!_context.TryGetGuildID(out var guildId))
            return await _feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        if (!member.User.IsDefined(out var memberUser))
            return await _feedbackService.SendContextualErrorAsync("Could not get user.");

        if (!_discord.GatewayCache.GetGuildPresence(guildId.Value, memberUser.ID).IsDefined(out var presence))
            return await _feedbackService.SendContextualErrorAsync($"Cannot find presence for {member.Mention()}.");

        var presenceJson = JsonSerializer.Serialize(presence, _discord.JsonSerializerOptions);
        return await _feedbackService.SendContextualSuccessAsync($"Presence for {member.Mention()}\n```{presenceJson}```");
    }

    [Command("user", "member", "whois")]
    [Description("Get info for a user.")]
    public async Task<IResult> UserInfoAsync([Description("User to get info for")] IUser user)
    {
        if (!_context.TryGetGuildID(out var guildId))
            goto USER;

        var memberResult = await _discord.Rest.Guild.GetGuildMemberAsync(guildId.Value, user.ID);
        if (memberResult.IsDefined(out var member))
            return await _feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(member, _discord.JsonSerializerOptions)}```");

    USER:
        return await _feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(user, _discord.JsonSerializerOptions)}```");
    }
}
