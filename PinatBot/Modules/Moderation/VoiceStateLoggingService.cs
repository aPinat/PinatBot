using System.Drawing;
using Microsoft.EntityFrameworkCore;
using PinatBot.Caching.VoiceStates;
using PinatBot.Data;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Extensions.Embeds;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class VoiceStateLoggingService
{
    public VoiceStateLoggingService(IDbContextFactory<Database> dbContextFactory, VoiceStateCacheService voiceStateCache, IDiscordRestChannelAPI channelApi)
    {
        DbContextFactory = dbContextFactory;
        VoiceStateCache = voiceStateCache;
        ChannelApi = channelApi;
    }

    private IDbContextFactory<Database> DbContextFactory { get; }
    private VoiceStateCacheService VoiceStateCache { get; }
    private IDiscordRestChannelAPI ChannelApi { get; }

    internal async Task<Result> LogVoiceStateUpdateAsync(IVoiceStateUpdate vsu, CancellationToken cancellationToken)
    {
        if (!vsu.GuildID.IsDefined(out var guildId) || !vsu.Member.IsDefined(out var member) || !member.User.IsDefined(out var user) || (user.IsBot.IsDefined(out var isBot) && isBot))
            return Result.FromSuccess();

        var cacheResult = VoiceStateCache.Get(guildId, vsu.UserID);
        if (cacheResult.IsDefined(out var oldVoiceState) && oldVoiceState.ChannelID == vsu.ChannelID)
            return Result.FromSuccess();

        await using var database = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.VoiceStateLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == guildId.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var builder = new EmbedBuilder
        {
            Timestamp = DateTimeOffset.Now, Author = new EmbedAuthorBuilder(user.DiscordTag(), iconUrl: user.AvatarUrl().ToString()), Footer = new EmbedFooterBuilder("Member ID: " + user.ID)
        };

        if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.HasValue is null or false)
        {
            var channelResult = await ChannelApi.GetChannelAsync(vsu.ChannelID.Value, cancellationToken);
            if (!channelResult.IsDefined(out var channel))
                return Result.FromError(channelResult);

            builder.Colour = Color.Lime;
            builder.Description = $"{user.Mention()} **joined** `#{channel.Name.Value}` ({channel.Mention()})";
        }
        else if (!vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.HasValue is true)
        {
            var channelResult = await ChannelApi.GetChannelAsync(oldVoiceState.ChannelID.Value, cancellationToken);
            if (!channelResult.IsDefined(out var channel))
                return Result.FromError(channelResult);

            builder.Colour = Color.Red;
            builder.Description = $"{user.Mention()} **left** `#{channel.Name.Value}` ({channel.Mention()})";
        }
        else if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.HasValue is true)
        {
            var oldChannelTask = ChannelApi.GetChannelAsync(oldVoiceState.ChannelID.Value, cancellationToken);
            var newChannelTask = ChannelApi.GetChannelAsync(vsu.ChannelID.Value, cancellationToken);

            var oldChannelResult = await oldChannelTask;
            if (!oldChannelResult.IsDefined(out var oldChannel))
                return Result.FromError(oldChannelResult);

            var newChannelResult = await newChannelTask;
            if (!newChannelResult.IsDefined(out var newChannel))
                return Result.FromError(newChannelResult);

            builder.Colour = Color.Orange;
            builder.Description = $"{user.Mention()} **moved** `#{oldChannel.Name.Value}` ==> `#{newChannel.Name.Value}` ({oldChannel.Mention()} ==> {newChannel.Mention()})";
        }
        else
        {
            return Result.FromError(new InvalidOperationError("Invalid voice state update."));
        }

        var buildResult = builder.Build();
        if (!buildResult.IsDefined(out var embed))
            return Result.FromError(buildResult);

        var messageResult = await ChannelApi.CreateMessageAsync(new Snowflake(logging.ChannelId), embeds: new[] { embed }, ct: cancellationToken);
        return messageResult.IsSuccess ? Result.FromSuccess() : Result.FromError(messageResult);
    }
}