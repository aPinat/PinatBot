using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PinatBot.CommandHelpers;
using PinatBot.Data;
using Remora.Discord.Commands.Services;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Results;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot;

public sealed class PinatBot(
    IHostEnvironment hostEnvironment,
    Configuration configuration,
    ILogger<PinatBot> logger,
    IDbContextFactory<Database> dbContextFactory,
    DiscordGatewayClient discordGatewayClient,
    SlashService slashService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var isDevelopment = hostEnvironment.IsDevelopment();
        var testGuild = configuration.Discord.TestGuild;

        await using var database = await dbContextFactory.CreateDbContextAsync(stoppingToken);
        await database.Database.MigrateAsync(stoppingToken);

        if (isDevelopment && testGuild.HasValue)
        {
            var updateSlash = await slashService.UpdateSlashCommandsAsync(new Snowflake(testGuild.Value), TreeNameResolver.InteractionCommandTreeName, stoppingToken);
            if (!updateSlash.IsSuccess)
                logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }
        else
        {
            var updateSlash = await slashService.UpdateSlashCommandsAsync(treeName: TreeNameResolver.InteractionCommandTreeName, ct: stoppingToken);
            if (!updateSlash.IsSuccess)
                logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var runResult = await discordGatewayClient.RunAsync(stoppingToken);
            if (runResult.IsSuccess)
                continue;

            switch (runResult.Error)
            {
                case ExceptionError e:
                    if (e.Exception is OperationCanceledException)
                        continue;
                    logger.LogError
                        (e.Exception, "Exception during gateway connection: {ExceptionMessage}", e.Message);
                    break;
                case GatewayWebSocketError:
                case GatewayDiscordError:
                case GatewayError:
                    logger.LogError("Gateway error: {Message}", runResult.Error.Message);
                    break;
                default:
                    logger.LogError("Unknown error: {Message}", runResult.Error?.Message);
                    break;
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
