using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

public sealed class PinatBot : BackgroundService
{
    private readonly Configuration _configuration;
    private readonly IDbContextFactory<Database> _dbContextFactory;
    private readonly DiscordGatewayClient _discordGatewayClient;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<PinatBot> _logger;
    private readonly SlashService _slashService;

    public PinatBot(IHostEnvironment hostEnvironment,
        IConfiguration configuration,
        ILogger<PinatBot> logger,
        IDbContextFactory<Database> dbContextFactory,
        DiscordGatewayClient discordGatewayClient,
        SlashService slashService)
    {
        _hostEnvironment = hostEnvironment;
        _configuration = configuration.Get<Configuration>();
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _discordGatewayClient = discordGatewayClient;
        _slashService = slashService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var isDevelopment = _hostEnvironment.IsDevelopment();
        var testGuild = _configuration.Discord.TestGuild;

        await using var database = await _dbContextFactory.CreateDbContextAsync(stoppingToken);
        await database.Database.MigrateAsync(stoppingToken);

        if (isDevelopment && testGuild.HasValue)
        {
            var updateSlash = await _slashService.UpdateSlashCommandsAsync(new Snowflake(testGuild.Value), TreeNameResolver.InteractionCommandTreeName, stoppingToken);
            if (!updateSlash.IsSuccess)
                _logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }
        else
        {
            var updateSlash = await _slashService.UpdateSlashCommandsAsync(treeName: TreeNameResolver.InteractionCommandTreeName, ct: stoppingToken);
            if (!updateSlash.IsSuccess)
                _logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var runResult = await _discordGatewayClient.RunAsync(stoppingToken);
            if (runResult.IsSuccess)
                continue;

            switch (runResult.Error)
            {
                case ExceptionError e:
                    if (e.Exception is OperationCanceledException)
                        continue;
                    _logger.LogError
                        (e.Exception, "Exception during gateway connection: {ExceptionMessage}", e.Message);
                    break;
                case GatewayWebSocketError:
                case GatewayDiscordError:
                case GatewayError:
                    _logger.LogError("Gateway error: {Message}", runResult.Error.Message);
                    break;
                default:
                    _logger.LogError("Unknown error: {Message}", runResult.Error?.Message);
                    break;
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
