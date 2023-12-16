using System.Drawing;
using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Extensions.Embeds;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class VoiceStateLoggingService(IDbContextFactory<Database> dbContextFactory, Discord discord)
{
    internal async Task<Result> LogVoiceStateUpdateAsync(IVoiceStateUpdate vsu, CancellationToken cancellationToken)
    {
        if (!vsu.GuildID.IsDefined(out var guildId) || !vsu.Member.IsDefined(out var member) || !member.User.IsDefined(out var user) || (user.IsBot.IsDefined(out var isBot) && isBot))
            return Result.FromSuccess();

        var cacheResult = discord.GatewayCache.GetVoiceState(guildId, vsu.UserID);
        if (cacheResult.IsDefined(out var oldVoiceState) && oldVoiceState.ChannelID.IsDefined(out var channelID) && channelID == vsu.ChannelID)
            return Result.FromSuccess();

        await using var database = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.VoiceStateLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == guildId.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var builder = new EmbedBuilder
        {
            Timestamp = DateTimeOffset.Now, Author = new EmbedAuthorBuilder(user.DiscordTag(), iconUrl: user.AvatarUrl().ToString()), Footer = new EmbedFooterBuilder("Member ID: " + user.ID)
        };

        if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.Value.HasValue is null or false)
        {
            var channelResult = discord.GatewayCache.GetChannel(guildId, vsu.ChannelID.Value);
            if (!channelResult.IsDefined(out var channel))
                return Result.FromError(channelResult);

            builder.Colour = Color.Lime;
            builder.Description = $"{user.Mention()} `{user.DiscordTag()}` **joined** `#{channel.Name.Value}` ({channel.Mention()})";
        }
        else if (!vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.IsDefined(out var id) is true)
        {
            var channelResult = discord.GatewayCache.GetChannel(guildId, id.Value);
            if (!channelResult.IsDefined(out var channel))
                return Result.FromError(channelResult);

            builder.Colour = Color.Red;
            builder.Description = $"{user.Mention()} `{user.DiscordTag()}` **left** `#{channel.Name.Value}` ({channel.Mention()})";
        }
        else if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.IsDefined(out var id2) is true)
        {
            var oldChannelResult = discord.GatewayCache.GetChannel(guildId, id2.Value);
            if (!oldChannelResult.IsDefined(out var oldChannel))
                return Result.FromError(oldChannelResult);

            var newChannelResult = discord.GatewayCache.GetChannel(guildId, vsu.ChannelID.Value);
            if (!newChannelResult.IsDefined(out var newChannel))
                return Result.FromError(newChannelResult);

            builder.Colour = Color.Orange;
            builder.Description = $"{user.Mention()} `{user.DiscordTag()}` **moved** `#{oldChannel.Name.Value}` ==> `#{newChannel.Name.Value}` ({oldChannel.Mention()} ==> {newChannel.Mention()})";
        }
        else
        {
            return Result.FromError(new InvalidOperationError("Invalid voice state update."));
        }

        var buildResult = builder.Build();
        if (!buildResult.IsDefined(out var embed))
            return Result.FromError(buildResult);

        var messageResult = await discord.Rest.Channel.CreateMessageAsync(new Snowflake(logging.ChannelId), embeds: new[] { embed }, ct: cancellationToken);
        return messageResult.IsSuccess ? Result.FromSuccess() : Result.FromError(messageResult);
    }
}
