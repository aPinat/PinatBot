using Microsoft.Extensions.Logging;
using OneOf;
using Remora.Commands.Results;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Commands.Services;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PostCommandExecutionHandler : IPostExecutionEvent
{
    private readonly Discord _discord;
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<PostCommandExecutionHandler> _logger;

    public PostCommandExecutionHandler(ILogger<PostCommandExecutionHandler> logger, FeedbackService feedbackService, Discord discord)
    {
        _logger = logger;
        _feedbackService = feedbackService;
        _discord = discord;
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
                    if (!interactionContext.Data.TryPickT0(out var applicationCommandData, out _))
                    {
                        _logger.LogWarning("Interaction is not of type IApplicationCommandData, but of type {Type}", interactionContext.Data.Value.GetType());
                        goto SUCCESS;
                    }

                    applicationCommandData.UnpackInteraction(out var commandPath, out var parameters);
                    command = string.Join(' ', commandPath) +
                              (parameters.Any() ? " " : string.Empty) +
                              string.Join(' ', parameters.Select(pair => pair.Key + " = [" + string.Join(" ", pair.Value) + "]"));
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

        if (context is InteractionContext { HasRespondedToInteraction: false } interaction)
            await _discord.Rest.Interaction.CreateInteractionResponseAsync(interaction.ID, interaction.Token,
                new InteractionResponse(InteractionCallbackType.DeferredChannelMessageWithSource,
                    new Optional<OneOf<IInteractionMessageCallbackData, IInteractionAutocompleteCallbackData, IInteractionModalCallbackData>>(
                        new InteractionMessageCallbackData(Flags: MessageFlags.Ephemeral))), ct: ct);

        Result<IReadOnlyList<IMessage>> replyResult = default;
        switch (error)
        {
            case CommandNotFoundError:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but the command was not found",
                    user.DiscordTag(), user.ID, type, command);
                if (context is InteractionContext)
                    replyResult = await _feedbackService.SendContextualErrorAsync("The command you tried to execute was not found.", ct: ct);
                break;
            case ConditionNotSatisfiedError e:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but a condition was not satisfied: {ErrorMessage} {ConditionErrorMessage}",
                    user.DiscordTag(), user.ID, type, command, e.Message, commandResult.Inner?.Inner?.Inner?.Inner?.Error?.Message);
                replyResult = await _feedbackService.SendContextualErrorAsync($"Your command could not be executed.\n{commandResult.Inner?.Inner?.Inner?.Inner?.Error?.Message}", ct: ct);
                break;
            case ParameterParsingError e:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}' but a parameter could not be parsed: {ParameterName}",
                    user.DiscordTag(), user.ID, type, command, e.Parameter.ParameterShape.HintName);
                replyResult = await _feedbackService.SendContextualErrorAsync($"Could not parse your input for `{e.Parameter.ParameterShape.HintName}`. Check your input and try again.", ct: ct);
                break;
            case ExceptionError e:
                _logger.LogError(e.Exception, "Exception occured while {UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}'",
                    user.DiscordTag(), user.ID, type, command);
                replyResult = await _feedbackService.SendContextualErrorAsync("An exception occured while executing your command. Please try again later.", ct: ct);
                break;
            default:
                _logger.LogWarning("{UserDiscordTag} ({UserId}) tried to execute {CommandType} command '{Command}', but errored with {ErrorType}: {ErrorMessage}'",
                    user.DiscordTag(), user.ID, type, command, error?.GetType().Name, error?.Message);
                replyResult = await _feedbackService.SendContextualErrorAsync("An error occured while executing your command. Please try again later.", ct: ct);
                break;
        }

        if (!replyResult.IsSuccess)
            return Result.FromError(replyResult);

    SUCCESS:
        return Result.FromSuccess();
    }
}
