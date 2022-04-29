using Microsoft.Extensions.DependencyInjection;
using PinatBot.Modules.General.Commands;
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
            .AddDiscordCommands(true)
            .AddCommandTree()
            .WithCommandGroup<GeneralCommands>()
            .Finish()
            .AddInteractivity()
            .AddPagination()
            .AddPostExecutionEvent<PostCommandExecutionHandler>();
}
