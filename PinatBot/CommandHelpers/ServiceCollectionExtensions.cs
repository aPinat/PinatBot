using Microsoft.Extensions.DependencyInjection;
using PinatBot.Modules.General.Commands;
using PinatBot.Modules.Moderation.Commands;
using PinatBot.Modules.Music.Commands;
using Remora.Commands.Extensions;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Services;
using Remora.Discord.Interactivity.Extensions;
using Remora.Discord.Pagination.Extensions;

namespace PinatBot.CommandHelpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPinatBotCommands(this IServiceCollection services) =>
        services
            .AddTransient<ICommandPrefixMatcher, PrefixMatcher>()
            .AddSingleton<ITreeNameResolver, TreeNameResolver>()
            .AddDiscordCommands(true)
            .AddCommandTree(TreeNameResolver.MessageCommandTreeName)
            .WithCommandGroup<GeneralCommands>()
            .WithCommandGroup<MusicCommands>()
            .Finish()
            .AddCommandTree(TreeNameResolver.InteractionCommandTreeName)
            .WithCommandGroup<GeneralCommands>()
            .WithCommandGroup<LoggingCommands>()
            .WithCommandGroup<MemberJoinRoleCommands>()
            .WithCommandGroup<ModerationCommands>()
            .WithCommandGroup<MusicCommands>()
            .Finish()
            .AddInteractivity()
            .AddPagination()
            .AddPreparationErrorEvent<CommandPreparationErrorHandler>()
            .AddPostExecutionEvent<PostCommandExecutionHandler>();
}
