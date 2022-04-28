using Microsoft.Extensions.Logging;
using Remora.Commands.Results;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PostCommandExecutionHandler : IPostExecutionEvent
{
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<PostCommandExecutionHandler> _logger;

    public PostCommandExecutionHandler(ILogger<PostCommandExecutionHandler> logger, FeedbackService feedbackService)
    {
        _logger = logger;
        _feedbackService = feedbackService;
    }

    public async Task<Result> AfterExecutionAsync(ICommandContext context, IResult commandResult, CancellationToken ct = default)
    {
        var user = context.User;
        string command, type;
        switch (context)
        {
            case MessageContext messageContext:
                type = "message";
                command = messageContext.Message.Content.HasValue ? messageContext.Message.Content.Value : string.Empty;
                break;
            case InteractionContext interactionContext:
                {
                    type = "interaction";
                    interactionContext.Data.UnpackInteraction(out var commandPath, out var parameters);
                    command = string.Join(' ', commandPath) + string.Join(';', parameters.Select(pair => pair.Key + " = [" + string.Join(" ", pair.Value) + "]"));
                    break;
                }
            default:
                goto SUCCESS;
        }

        if (commandResult.IsSuccess)
        {
            _logger.LogInformation("{UserDiscordTag} ({UserId}) successfully executed {CommandType} command '{Command}'", user.DiscordTag(), user.ID, type, command);
            goto SUCCESS;
        }

        var error = commandResult.Error;

        while (error is AggregateError aggregateError)
            error = aggregateError.Errors.First().Error;

        switch (error)
        {
            case CommandNotFoundError:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but the command was not found",
                    user.DiscordTag(), user.ID, type, command);
                break;
            case ConditionNotSatisfiedError e:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but a condition was not satisfied: {ConditionErrorMessage}",
                    user.DiscordTag(), user.ID, type, command, e.Message);
                await _feedbackService.SendContextualErrorAsync($"Your command could not be executed because a condition was not satisfied. {e.Message}", ct: ct);
                break;
            case ParameterParsingError e:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but a parameter could not be parsed: {ParameterName}",
                    user.DiscordTag(), user.ID, type, command, e.Parameter.ParameterShape.HintName);
                await _feedbackService.SendContextualErrorAsync($"Could not parse your input for `{e.Parameter.ParameterShape.HintName}`. Check your input and try again.", ct: ct);
                break;
            case ExceptionError e:
                _logger.LogError(e.Exception, "Exception occured while {UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}'",
                    user.DiscordTag(), user.ID, type, command);
                await _feedbackService.SendContextualErrorAsync("An exception occured while executing your command. Please try again later.", ct: ct);
                break;
            default:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}', but errored with {ErrorType}: {ErrorMessage}'",
                    user.DiscordTag(), user.ID, type, command, error?.GetType().Name, error?.Message);
                await _feedbackService.SendContextualErrorAsync("An error occured while executing your command. Please try again later.", ct: ct);
                break;
        }

        SUCCESS:
        return Result.FromSuccess();
    }
}
