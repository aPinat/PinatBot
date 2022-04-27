using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway;
using Remora.Results;

namespace PinatBot.Modules.General.Commands;

public class PingCommand : CommandGroup
{
    public PingCommand(FeedbackService feedbackService, DiscordGatewayClient discordGatewayClient)
    {
        FeedbackService = feedbackService;
        DiscordGatewayClient = discordGatewayClient;
    }

    private FeedbackService FeedbackService { get; }
    private DiscordGatewayClient DiscordGatewayClient { get; }


    [Command("ping")]
    [Description("Get ping between bot and Discord websocket.")]
    public async Task<IResult> PingAsync()
        => await FeedbackService.SendContextualSuccessAsync($"Pong!\nPing to Discord WebSocket: {DiscordGatewayClient.Latency.Milliseconds}ms");
}
