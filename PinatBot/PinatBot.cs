using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PinatBot.Data;
using Remora.Discord.Commands.Services;
using Remora.Rest.Core;

namespace PinatBot;

public sealed class PinatBot : IHostedService
{
    public PinatBot(IHostEnvironment hostEnvironment, IConfiguration configuration, ILogger<PinatBot> logger, IDbContextFactory<Database> dbContextFactory,
        SlashService slashService)
    {
        HostEnvironment = hostEnvironment;
        Configuration = configuration.Get<Configuration>();
        Logger = logger;
        DbContextFactory = dbContextFactory;
        SlashService = slashService;
    }

    private Configuration Configuration { get; }
    private IHostEnvironment HostEnvironment { get; }
    private ILogger<PinatBot> Logger { get; }
    private IDbContextFactory<Database> DbContextFactory { get; }
    private SlashService SlashService { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var isDevelopment = HostEnvironment.IsDevelopment();
        var testGuild = Configuration.Discord.TestGuild;

        await using var database = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        await database.Database.MigrateAsync(cancellationToken);

        var checkSlashSupport = SlashService.SupportsSlashCommands();
        if (!checkSlashSupport.IsSuccess)
        {
            Logger.LogWarning("The registered commands of the bot don't support slash commands: {Reason}", checkSlashSupport.Error?.Message);
        }
        else if (isDevelopment && testGuild.HasValue)
        {
            var updateSlash = await SlashService.UpdateSlashCommandsAsync(new Snowflake(testGuild.Value), ct: cancellationToken);
            if (!updateSlash.IsSuccess)
                Logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }
        else
        {
            var updateSlash = await SlashService.UpdateSlashCommandsAsync(ct: cancellationToken);
            if (!updateSlash.IsSuccess)
                Logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error?.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
