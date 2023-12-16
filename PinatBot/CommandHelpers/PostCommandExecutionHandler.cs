using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Commands.Services;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PostCommandExecutionHandler(ILogger<PostCommandExecutionHandler> logger, FeedbackService feedbackService) : IPostExecutionEvent
{
    public async Task<Result> AfterExecutionAsync(ICommandContext context, IResult commandResult, CancellationToken ct = default)
    {
        IUser user = new User(new Snowflake(0), "<UNKNOWN>", 0000, "<UNKNOWN>", default);
        string command, type;
        switch (context)
        {
            case IMessageContext messageContext:
                type = "message";

                if (messageContext.Message.Author.HasValue)
                    user = messageContext.Message.Author.Value;

                command = messageContext.Message.Content.HasValue ? messageContext.Message.Content.Value : string.Empty;
                break;
            case IInteractionContext interactionContext:
                type = "interaction";

                if (interactionContext.Interaction.User.HasValue)
                    user = interactionContext.Interaction.User.Value;
                else if (interactionContext.Interaction.Member.IsDefined(out var member) && member.User.HasValue)
                    user = member.User.Value;

                if (!interactionContext.Interaction.Data.Value.TryPickT0(out var applicationCommandData, out _))
                {
                    logger.LogWarning("Interaction is not of type IApplicationCommandData, but of type {Type}", interactionContext.Interaction.Data.Value.GetType());
                    goto SUCCESS;
                }

                applicationCommandData.UnpackInteraction(out var commandPath, out var parameters);
                command = string.Join(' ', commandPath) +
                          (parameters.Any() ? " " : string.Empty) +
                          string.Join(' ', parameters.Select(pair => pair.Key + " = [" + string.Join(" ", pair.Value) + "]"));
                break;
            default:
                goto SUCCESS;
        }

        if (commandResult.IsSuccess)
        {
            logger.LogInformation("{UserDiscordTag} ({UserId}) successfully executed {CommandType} command '{Command}'", user.DiscordTag(), user.ID, type, command);
            goto SUCCESS;
        }

        var error = commandResult.Error;

        while (error is AggregateError aggregateError)
            error = aggregateError.Errors.First().Error;

        Result<IReadOnlyList<IMessage>> replyResult;
        switch (error)
        {
            case ExceptionError e:
                logger.LogError(e.Exception, "Exception occured while {UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}'",
                    user.DiscordTag(), user.ID, type, command);
                replyResult = await feedbackService.SendContextualErrorAsync("An exception occured while executing your command. Please try again later.", ct: ct);
                break;
            default:
                logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}', but errored with {ErrorType}: {ErrorMessage}'",
                    user.DiscordTag(), user.ID, type, command, error?.GetType().Name, error?.Message);
                replyResult = await feedbackService.SendContextualErrorAsync("An error occured while executing your command. Please try again later.", ct: ct);
                break;
        }

        if (!replyResult.IsSuccess)
            return Result.FromError(replyResult);

    SUCCESS:
        return Result.FromSuccess();
    }
}
