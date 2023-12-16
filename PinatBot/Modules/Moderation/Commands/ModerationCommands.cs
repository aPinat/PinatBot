using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.Moderation.Commands;

[DiscordDefaultDMPermission(false)]
public class ModerationCommands(IOperationContext commandContext, IFeedbackService feedbackService, Discord discord) : CommandGroup
{
    [Command("purge")]
    [Description("Purge a number of messages in this channel.")]
    [DiscordDefaultMemberPermissions(DiscordPermission.ManageMessages)]
    [RequireDiscordPermission(DiscordPermission.ManageMessages)]
    [RequireBotDiscordPermissions(DiscordPermission.ManageMessages)]
    public async Task<IResult> PurgeAsync([Description("Number of messages to purge")] int count)
    {
        if (!commandContext.TryGetChannelID(out var channelId))
            return Result.FromError(new InvalidOperationError("Could not get channel."));

        await feedbackService.SendContextualInfoAsync($"Purging {count} messages...");
        var messagesResult = await discord.Rest.Channel.GetChannelMessagesAsync(channelId, limit: count + 1);
        if (!messagesResult.IsDefined(out var messages))
            return Result.FromError(messagesResult);

        if (messages.Count == 1)
            return await discord.Rest.Channel.DeleteMessageAsync(channelId, messages[0].ID);

        return await discord.Rest.Channel.BulkDeleteMessagesAsync(channelId, messages.Select(message => message.ID).ToArray());
    }
}
