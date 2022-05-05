using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.Moderation.Commands;

[DiscordDefaultDMPermission(false)]
public class ModerationCommands : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly Discord _discord;
    private readonly FeedbackService _feedbackService;

    public ModerationCommands(ICommandContext commandContext, FeedbackService feedbackService, Discord discord)
    {
        _commandContext = commandContext;
        _feedbackService = feedbackService;
        _discord = discord;
    }

    [Command("purge")]
    [Description("Purge a number of messages in this channel.")]
    [DiscordDefaultMemberPermissions(DiscordPermission.ManageMessages)]
    [RequireDiscordPermission(DiscordPermission.ManageMessages)]
    [RequireBotDiscordPermissions(DiscordPermission.ManageMessages)]
    public async Task<IResult> PurgeAsync([Description("Number of messages to purge")] int count)
    {
        await _feedbackService.SendContextualInfoAsync($"Purging {count} messages...");
        var messagesResult = await _discord.Rest.Channel.GetChannelMessagesAsync(_commandContext.ChannelID, limit: count + 1);
        if (!messagesResult.IsDefined(out var messages))
            return Result.FromError(messagesResult);

        if (messages.Count == 1)
            return await _discord.Rest.Channel.DeleteMessageAsync(_commandContext.ChannelID, messages[0].ID);

        return await _discord.Rest.Channel.BulkDeleteMessagesAsync(_commandContext.ChannelID, messages.Select(message => message.ID).ToArray());
    }
}
