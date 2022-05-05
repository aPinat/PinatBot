using System.ComponentModel;
using System.Text.Json;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
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
    public async Task<IResult> GameAsync([Description("User to get game for")] IUser user)
    {
        var result = _discord.Cache.Presences.Get(user.ID);

        if (!result.IsDefined(out var presence))
            return await _feedbackService.SendContextualNeutralAsync($"{user.Mention()} is not currently playing anything.");

        var activity = presence.Activities?.FirstOrDefault(activity => activity.Type == ActivityType.Game);
        return activity is null
            ? await _feedbackService.SendContextualNeutralAsync($"{user.Mention()} is not currently playing anything.")
            : await _feedbackService.SendContextualSuccessAsync($"{user.Mention()} is currently playing {activity.Name}");
    }

    [Command("presence")]
    [Description("Get presence for user.")]
    public async Task<IResult> PresenceAsync([Description("User to get presence for")] IUser user)
    {
        var result = _discord.Cache.Presences.Get(user.ID);

        if (!result.IsDefined(out var presence))
            return await _feedbackService.SendContextualErrorAsync($"Cannot find presence for {user.Mention()}.");

        var presenceJson = JsonSerializer.Serialize(presence, _discord.JsonSerializerOptions);
        return await _feedbackService.SendContextualSuccessAsync($"Presence for {user.Mention()}\n```{presenceJson}```");
    }

    [Command("user", "member", "whois")]
    [Description("Get info about a user.")]
    public async Task<IResult> UserInfoAsync([Description("User to get info for")] IUser user)
    {
        if (!_context.GuildID.IsDefined(out var guildId))
            goto USER;

        var guildMember = await _discord.Rest.Guild.GetGuildMemberAsync(guildId, user.ID);
        if (guildMember.IsDefined(out var member))
            return await _feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(member, _discord.JsonSerializerOptions)}```");

        USER:
        return await _feedbackService.SendContextualSuccessAsync($"```{JsonSerializer.Serialize(user, _discord.JsonSerializerOptions)}```");
    }
}
