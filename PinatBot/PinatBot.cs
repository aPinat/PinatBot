using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PinatBot.Data;
using Remora.Discord.Commands.Services;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Results;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot;

public sealed class PinatBot : IHostedService
{
    private readonly Configuration _configuration;
    private readonly IDbContextFactory<Database> _dbContextFactory;
    private readonly DiscordGatewayClient _discordGatewayClient;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<PinatBot> _logger;
    private readonly SlashService _slashService;
    private Task<Result>? _discordGatewayClientTask;

    public PinatBot(IHostEnvironment hostEnvironment, IConfiguration configuration, ILogger<PinatBot> logger, IDbContextFactory<Database> dbContextFactory,
        DiscordGatewayClient discordGatewayClient, SlashService slashService)
    {
        _hostEnvironment = hostEnvironment;
        _configuration = configuration.Get<Configuration>();
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _discordGatewayClient = discordGatewayClient;
        _slashService = slashService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var isDevelopment = _hostEnvironment.IsDevelopment();
        var testGuild = _configuration.Discord.TestGuild;

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await database.Database.MigrateAsync(cancellationToken);

        var checkSlashSupport = _slashService.SupportsSlashCommands();
        if (!checkSlashSupport.IsSuccess)
        {
            _logger.LogWarning("The registered commands of the bot don't support slash commands: {Reason}", checkSlashSupport.Error?.Message);
        }
        else if (isDevelopment && testGuild.HasValue)
        {
            var updateSlash = await _slashService.UpdateSlashCommandsAsync(new Snowflake(testGuild.Value), ct: cancellationToken);
            if (!updateSlash.IsSuccess)
                _logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }
        else
        {
            var updateSlash = await _slashService.UpdateSlashCommandsAsync(ct: cancellationToken);
            if (!updateSlash.IsSuccess)
                _logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            _discordGatewayClientTask = _discordGatewayClient.RunAsync(cancellationToken);
            var runResult = await _discordGatewayClientTask;
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

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_discordGatewayClientTask is not null)
            await _discordGatewayClientTask;
    }
}
