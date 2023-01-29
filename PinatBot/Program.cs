using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PinatBot;
using PinatBot.Caching;
using PinatBot.CommandHelpers;
using PinatBot.Data;
using PinatBot.Modules.Moderation;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Gateway.Commands;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .ConfigureLogging(builder => builder.ClearProviders())
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .ConfigureServices((context, collection) =>
    {
        var isDevelopment = context.HostingEnvironment.IsDevelopment();
        var configuration = context.Configuration.Get<Configuration>();
        if (configuration is null)
            throw new InvalidOperationException("Configuration could not be bound to type...");

        collection
            .AddSingleton(configuration)
            .AddPooledDbContextFactory<Database>(options => options
                .EnableThreadSafetyChecks(isDevelopment)
                .EnableDetailedErrors(isDevelopment)
                .EnableSensitiveDataLogging(isDevelopment)
                .UseNpgsql(configuration.ConnectionStrings.Postgres)
                .UseSnakeCaseNamingConvention()
            )
            .AddDiscordGateway(_ => configuration.Discord.BotToken)
            .Configure<DiscordGatewayClientOptions>(options =>
            {
                options.Intents = GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildEmojisAndStickers | GatewayIntents.GuildVoiceStates | GatewayIntents.GuildPresences |
                                  GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContents;
                options.Presence = new UpdatePresence(UserStatus.DND, false, null, new[] { new Activity("#Pinats", ActivityType.Competing) });
            })
            .AddPinatBotCaching(options => options.Configuration = configuration.ConnectionStrings.Redis)
            .AddSingleton<Discord>()
            .AddSingleton<GeneralLoggingService>()
            .AddResponder<GeneralLoggingResponder>()
            .AddSingleton<MemberJoinRoleService>()
            .AddResponder<MemberJoinRoleResponder>()
            .AddSingleton<VoiceStateLoggingService>()
            .AddResponder<VoiceStateLoggingResponder>()
            .AddPinatBotCommands()
            .AddHostedService<PinatBot.PinatBot>();
    })
    .Build()
    .RunAsync();
