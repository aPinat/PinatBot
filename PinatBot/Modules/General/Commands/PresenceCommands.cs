using System.ComponentModel;
using PinatBot.Caching.Presences;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.General.Commands;

public class PresenceCommands : CommandGroup
{
    public PresenceCommands(FeedbackService feedbackService, PresenceCacheService cache)
    {
        FeedbackService = feedbackService;
        PresenceCache = cache;
    }

    private FeedbackService FeedbackService { get; }
    private PresenceCacheService PresenceCache { get; }

    [Command("game")]
    [Description("Get current game for user.")]
    public async Task<IResult> GameAsync([Description("User to get game for")] IUser user)
    {
        var result = PresenceCache.Get(user.ID);

        if (!result.IsDefined(out var presence))
            return await FeedbackService.SendContextualNeutralAsync($"{user.Mention()} is not currently playing anything.");

        var activity = presence.Activities?.FirstOrDefault(activity => activity.Type == ActivityType.Game);
        return activity is null
            ? await FeedbackService.SendContextualNeutralAsync($"{user.Mention()} is not currently playing anything.")
            : await FeedbackService.SendContextualSuccessAsync($"{user.Mention()} is currently playing {activity.Name}");
    }

    [Command("presence")]
    [Description("Get presence for user.")]
    public async Task<IResult> PresenceAsync([Description("User to get presence for")] IUser user)
    {
        var result = PresenceCache.Get(user.ID);

        if (!result.IsDefined(out var presence))
            return await FeedbackService.SendContextualErrorAsync($"Cannot find presence for {user.Mention()}.");

        return await FeedbackService.SendContextualSuccessAsync($"Presence for {user.Mention()}\n{presence}");
    }
}
