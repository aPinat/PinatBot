using System.Collections.Concurrent;
using System.Collections.Immutable;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using Remora.Results;
using Guild = PinatBot.Caching.Objects.Guild;
using Message = PinatBot.Caching.Objects.Message;

namespace PinatBot.Caching;

public class DiscordGatewayCache(DistributedCacheProvider distributedCacheProvider)
{
    internal readonly ConcurrentDictionary<ulong, Guild> InternalGuilds = new();

    internal readonly ConcurrentDictionary<ulong, Message> InternalMessages = new();

    internal readonly ConcurrentDictionary<ulong, IUser> InternalUsers = new();

    internal IUser? CurrentUser { get; set; }

    public Result<IUser> GetCurrentUser() =>
        CurrentUser is not null
            ? Result<IUser>.FromSuccess(CurrentUser)
            : Result<IUser>.FromError(new NotFoundError("CurrentUser is not cached"));

    public Result<IUser> GetUser(Snowflake userID) =>
        InternalUsers.TryGetValue(userID.Value, out var user)
            ? Result<IUser>.FromSuccess(user)
            : Result<IUser>.FromError(new NotFoundError("User not found in cache"));

    public Result<IGuildCreate.IAvailableGuild> GetGuild(Snowflake guildID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild)
            ? Result<IGuildCreate.IAvailableGuild>.FromSuccess(guild)
            : Result<IGuildCreate.IAvailableGuild>.FromError(new NotFoundError("Guild not found in cache"));

    public async Task<Result<IMessage>> GetMessageAsync(Snowflake messageID, Snowflake channelID, CancellationToken cancellationToken = default)
    {
        if (InternalMessages.TryGetValue(messageID.Value, out var message))
            return message;

        var key = DistributedCacheProvider.CreateMessageCacheKey(channelID, messageID);
        var cachedMessageDistributed = await distributedCacheProvider.RetrieveAsync<IMessage>(key, cancellationToken);
        return cachedMessageDistributed is null ? Result<IMessage>.FromError(new NotFoundError("Message not found in cache")) : Result<IMessage>.FromSuccess(cachedMessageDistributed);
    }

    public Task CacheMessageAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        var m = new Message(message.ID, message.ChannelID);
        m.Populate(message);
        InternalMessages[message.ID.Value] = m;

        var key = DistributedCacheProvider.CreateMessageCacheKey(message.ChannelID, message.ID);
        return distributedCacheProvider.CacheAsync<IMessage>(key, m, cancellationToken);
    }

    public Result<IPartialVoiceState> GetVoiceState(Snowflake guildID, Snowflake userID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild) && guild.VoiceStatesInternal.TryGetValue(userID.Value, out var voiceState)
            ? Result<IPartialVoiceState>.FromSuccess(voiceState)
            : Result<IPartialVoiceState>.FromError(new NotFoundError("Voice state not found in cache"));

    public Result<IChannel> GetChannel(Snowflake guildID, Snowflake channelID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild) && guild.ChannelsInternal.TryGetValue(channelID.Value, out var channel)
            ? Result<IChannel>.FromSuccess(channel)
            : Result<IChannel>.FromError(new NotFoundError("Channel not found in cache"));

    public Result<IChannel> GetChannel(Snowflake channelID)
    {
        foreach (var guild in InternalGuilds.Values)
            if (guild.ChannelsInternal.TryGetValue(channelID.Value, out var channel))
                if (channel is Channel channelRecord)
                    return Result<IChannel>.FromSuccess(channelRecord with { GuildID = guild.ID });
                else
                    return Result<IChannel>.FromSuccess(channel);

        return Result<IChannel>.FromError(new NotFoundError("Channel not found in cache"));
    }

    public Result<IReadOnlyList<IChannel>> GetGuildChannels(Snowflake guildID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild)
            ? Result<IReadOnlyList<IChannel>>.FromSuccess(guild.ChannelsInternal.Values.ToImmutableList())
            : Result<IReadOnlyList<IChannel>>.FromError(new NotFoundError("Guild not found in cache"));

    public Result<IGuildMember> GetGuildMember(Snowflake guildID, Snowflake userID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild) && guild.MembersInternal.TryGetValue(userID.Value, out var member)
            ? Result<IGuildMember>.FromSuccess(member)
            : Result<IGuildMember>.FromError(new NotFoundError("Member not found in cache"));

    public Result<IReadOnlyList<IRole>> GetGuildRoles(Snowflake guildID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild)
            ? Result<IReadOnlyList<IRole>>.FromSuccess(guild.RolesInternal.Values.ToImmutableList())
            : Result<IReadOnlyList<IRole>>.FromError(new NotFoundError("Guild not found in cache"));

    public Result<IPartialPresence> GetGuildPresence(Snowflake guildID, Snowflake userID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild) && guild.PresencesInternal.TryGetValue(userID.Value, out var presence)
            ? Result<IPartialPresence>.FromSuccess(presence)
            : Result<IPartialPresence>.FromError(new NotFoundError("Presence not found in cache"));

    public Result<IEmoji> GetGuildEmoji(Snowflake guildID, Snowflake emojiID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild) && guild.EmojisInternal.TryGetValue(emojiID.Value, out var emoji)
            ? Result<IEmoji>.FromSuccess(emoji)
            : Result<IEmoji>.FromError(new NotFoundError("Emoji not found in cache"));

    public Result<IReadOnlyList<IEmoji>> GetGuildEmojis(Snowflake guildID) =>
        InternalGuilds.TryGetValue(guildID.Value, out var guild)
            ? Result<IReadOnlyList<IEmoji>>.FromSuccess(guild.EmojisInternal.Values.ToImmutableList())
            : Result<IReadOnlyList<IEmoji>>.FromError(new NotFoundError("Guild not found in cache"));

    public Result<ISticker> GetGuildSticker(Snowflake guildId, Snowflake stickerId) =>
        InternalGuilds.TryGetValue(guildId.Value, out var guild) && guild.StickersInternal.TryGetValue(stickerId.Value, out var sticker)
            ? Result<ISticker>.FromSuccess(sticker)
            : Result<ISticker>.FromError(new NotFoundError("Sticker not found in cache"));

    public Result<IReadOnlyList<ISticker>> GetGuildStickers(Snowflake guildId) =>
        InternalGuilds.TryGetValue(guildId.Value, out var guild)
            ? Result<IReadOnlyList<ISticker>>.FromSuccess(guild.StickersInternal.Values.ToImmutableList())
            : Result<IReadOnlyList<ISticker>>.FromError(new NotFoundError("Guild not found in cache"));
}
