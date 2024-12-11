using Microsoft.EntityFrameworkCore;
using PinatBot.Data;
using PinatBot.Data.Modules.AutoVoiceChannels;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.AutoVoiceChannels;

public class AutoVoiceChannelService(IDbContextFactory<Database> dbContextFactory, Discord discord)
{
    public async Task<Result> HandleVoiceStateUpdateAsync(IVoiceStateUpdate vsu, CancellationToken cancellationToken = default)
    {
        if (!vsu.GuildID.IsDefined(out var guildId))
            return Result.FromError(new InvalidOperationError("GuildID is not defined"));

        var cacheResult = discord.GatewayCache.GetVoiceState(guildId, vsu.UserID);
        if (cacheResult.IsDefined(out var oldVoiceState) && oldVoiceState.ChannelID.IsDefined(out var channelID) && channelID == vsu.ChannelID)
            return Result.FromSuccess();

        // We're blocking the Caching Responder, so we manually cache this one.
        var cacheVoiceStateResult = discord.GatewayCache.CacheVoiceState(guildId, vsu);
        if (!cacheVoiceStateResult.IsSuccess)
            return cacheVoiceStateResult;

        if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.Value.HasValue is null or false)
            return await HandleUserJoinAsync(guildId, vsu.ChannelID.Value, cancellationToken);

        if (!vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.IsDefined(out var channelId) is true)
            return await HandleUserLeaveAsync(guildId, channelId.Value, vsu.UserID, cancellationToken);

        if (vsu.ChannelID.HasValue && oldVoiceState?.ChannelID.IsDefined(out var channelId2) is true)
        {
            var results = await Task.WhenAll(HandleUserJoinAsync(guildId, vsu.ChannelID.Value, cancellationToken),
                HandleUserLeaveAsync(guildId, channelId2.Value, vsu.UserID, cancellationToken));
            return results.All(result => result.IsSuccess) ? Result.FromSuccess() : Result.FromError(new AggregateError(results.Cast<IResult>().ToArray()));
        }

        return Result.FromError(new InvalidOperationError("Invalid voice state update."));
    }

    private async Task<Result> HandleUserJoinAsync(Snowflake guildId, Snowflake channelId, CancellationToken cancellationToken = default)
    {
        await using var database = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var newSessionConfig = await database.NewSessionChannels.FirstOrDefaultAsync(channel => channel.ChannelId == channelId.Value, cancellationToken);
        if (newSessionConfig is null)
            return Result.FromSuccess();

        var newSessionChannelResult = await discord.Rest.Channel.GetChannelAsync(channelId, cancellationToken);
        if (!newSessionChannelResult.IsDefined(out var newSessionChannel))
            return Result.FromError(newSessionChannelResult);

        var voiceStatesResult = discord.GatewayCache.GetVoiceStates(guildId, channelId);
        if (!voiceStatesResult.IsDefined(out var voiceStates))
            return Result.FromError(voiceStatesResult);

        if (!voiceStates.Any())
            return Result.FromSuccess();

        var newVoiceChannelResult = await discord.Rest.Guild.CreateGuildChannelAsync(guildId, newSessionConfig.ChildName, ChannelType.GuildVoice,
            bitrate: newSessionChannel.Bitrate.HasValue ? newSessionChannel.Bitrate.Value : default,
            userLimit: newSessionChannel.UserLimit.HasValue ? newSessionChannel.UserLimit.Value : default,
            parentID: newSessionChannel.ParentID,
            ct: cancellationToken, reason: "User joined auto voice channel.");
        if (!newVoiceChannelResult.IsDefined(out var newVoiceChannel))
            return Result.FromError(newVoiceChannelResult);

        database.AutoVoiceChannels.Add(new AutoVoiceChannel(newVoiceChannel.ID.Value));
        await database.SaveChangesAsync(cancellationToken);

        voiceStatesResult = discord.GatewayCache.GetVoiceStates(guildId, channelId);
        if (!voiceStatesResult.IsDefined(out voiceStates))
            return Result.FromError(voiceStatesResult);

        await Task.WhenAll(voiceStates.Where(vs => vs.UserID.HasValue).Select(vs =>
            discord.Rest.Guild.ModifyGuildMemberAsync(guildId, vs.UserID.Value, channelID: newVoiceChannel.ID, reason: "User joined auto voice channel.", ct: cancellationToken)));

        return Result.FromSuccess();
    }

    private async Task<Result> HandleUserLeaveAsync(Snowflake guildId, Snowflake channelId, Snowflake userId, CancellationToken cancellationToken = default)
    {
        await using var database = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var autoVoiceChannel = await database.AutoVoiceChannels.FirstOrDefaultAsync(channel => channel.ChannelId == channelId.Value, cancellationToken);
        if (autoVoiceChannel is null)
            return Result.FromSuccess();

        var voiceStatesResult = discord.GatewayCache.GetVoiceStates(guildId, channelId);
        if (!voiceStatesResult.IsDefined(out var voiceStates))
            return Result.FromError(voiceStatesResult);

        if (voiceStates.Any(vs => vs.UserID != userId))
            return Result.FromSuccess();

        await discord.Rest.Channel.DeleteChannelAsync(channelId, "All users left auto voice channel.", cancellationToken);
        database.AutoVoiceChannels.Remove(autoVoiceChannel);
        await database.SaveChangesAsync(cancellationToken);

        return Result.FromSuccess();
    }
}
