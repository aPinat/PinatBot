using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.General.Commands;

public class GeneralCommands : CommandGroup
{
    public GeneralCommands(FeedbackService feedbackService) => FeedbackService = feedbackService;
    private FeedbackService FeedbackService { get; }

    [Command("echo", "say")]
    [Description("Echoes the message of the user.")]
    public async Task<IResult> SayAsync([Description("string to echo")] string echo) => await FeedbackService.SendContextualNeutralAsync(echo);

    [Command("userinfo", "user", "member", "whois")]
    [Description("Get info about a member.")]
    public async Task<IResult> UserInfoAsync([Description("Member to get info for")] IGuildMember member) =>
        member is GuildMember guildMember
            ? await FeedbackService.SendContextualSuccessAsync(guildMember.ToString())
            : await FeedbackService.SendContextualErrorAsync("Can't serialize user.");
}
