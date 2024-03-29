﻿using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using PinatBot.Data.Modules.Moderation;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace PinatBot.Modules.Moderation.Commands;

[Group("logging")]
[DiscordDefaultMemberPermissions(DiscordPermission.ManageGuild)]
[DiscordDefaultDMPermission(false)]
[RequireDiscordPermission(DiscordPermission.ManageGuild)]
public class LoggingCommands(IDbContextFactory<Database> dbContextFactory, IOperationContext commandContext, IFeedbackService feedbackService)
    : CommandGroup
{
    public enum LoggingType
    {
        General,
        Voice
    }

    private static async Task<LoggingConfig?> GetLoggingConfigAsync(LoggingType type, Database database, ulong guildId) =>
        type switch
        {
            LoggingType.General => await database.GeneralLoggingConfigs
                .FirstOrDefaultAsync(config => config.GuildId == guildId),
            LoggingType.Voice => await database.VoiceStateLoggingConfigs
                .FirstOrDefaultAsync(config => config.GuildId == guildId),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    [Command("show", "get")]
    [Description("Show logging channel.")]
    public async Task<IResult> GetLoggingAsync(LoggingType type)
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var logging = await GetLoggingConfigAsync(type, database, guildId.Value);
        if (logging is null)
            return await feedbackService.SendContextualInfoAsync($"No {type} logging channel set.");

        if (logging.Enabled)
            return await feedbackService.SendContextualInfoAsync($"{type} logging channel set to <#{logging.ChannelId}>");

        return await feedbackService.SendContextualInfoAsync($"{type} logging is disabled.");
    }

    [Command("set")]
    [Description("Set channel to send logs to.")]
    public async Task<IResult> SetLoggingAsync([Description("General or Voice logging")] LoggingType type, [Description("Channel to log to")] [ChannelTypes(ChannelType.GuildText)] IChannel channel)
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        if (channel.Type != ChannelType.GuildText)
            return await feedbackService.SendContextualErrorAsync("Channel must be a text channel.");

        if (channel.GuildID.Value != guildId)
            return await feedbackService.SendContextualErrorAsync("Channel must be in this server.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var logging = await GetLoggingConfigAsync(type, database, guildId.Value);
        if (logging is null)
        {
            switch (type)
            {
                case LoggingType.General:
                    logging = new GeneralLoggingConfig(guildId.Value) { ChannelId = channel.ID.Value, Enabled = true };
                    await database.GeneralLoggingConfigs.AddAsync((GeneralLoggingConfig)logging);
                    break;
                case LoggingType.Voice:
                    logging = new VoiceStateLoggingConfig(guildId.Value) { ChannelId = channel.ID.Value, Enabled = true };
                    await database.VoiceStateLoggingConfigs.AddAsync((VoiceStateLoggingConfig)logging);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        else
        {
            logging.ChannelId = channel.ID.Value;
            logging.Enabled = true;
        }

        await database.SaveChangesAsync();
        return await feedbackService.SendContextualSuccessAsync($"{type} logging channel set to {channel.Mention()}");
    }

    [Command("disable", "off", "none")]
    [Description("Disable logging.")]
    public async Task<IResult> DisableLoggingAsync(LoggingType type)
    {
        if (!commandContext.TryGetGuildID(out var guildId))
            return await feedbackService.SendContextualErrorAsync("This command can only be used in a guild.");

        await using var database = await dbContextFactory.CreateDbContextAsync();
        var logging = await GetLoggingConfigAsync(type, database, guildId.Value);
        if (logging is null)
            return await feedbackService.SendContextualErrorAsync($"No {type} logging channel set.");

        logging.Enabled = false;
        await database.SaveChangesAsync();
        return await feedbackService.SendContextualSuccessAsync($"{type} logging disabled.");
    }
}
