using OneOf;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestChannelAPI
{
    public Task<Result<IChannel>> ModifyChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<Stream> icon = default,
        Optional<ChannelType> type = default,
        Optional<int?> position = default,
        Optional<string?> topic = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<bool> isArchived = default,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<bool> isLocked = default,
        Optional<bool> isInvitable = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<string?> rtcRegion = default,
        Optional<ChannelFlags> flags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyChannelAsync(channelID, name, icon, type, position, topic, isNsfw, rateLimitPerUser, bitrate, userLimit, permissionOverwrites, parentID, videoQualityMode, isArchived,
            autoArchiveDuration, isLocked, isInvitable, defaultAutoArchiveDuration, rtcRegion, flags, reason, ct);

    public Task<Result<IChannel>> ModifyGroupDMChannelAsync(Snowflake channelID, Optional<string> name = default, Optional<Stream> icon = default, CancellationToken ct = default) =>
        _actual.ModifyGroupDMChannelAsync(channelID, name, icon, ct);

    public Task<Result<IChannel>> ModifyGuildTextChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<ChannelType> type = default,
        Optional<int?> position = default,
        Optional<string?> topic = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyGuildTextChannelAsync(channelID, name, type, position, topic, isNsfw, rateLimitPerUser, permissionOverwrites, parentID, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> ModifyGuildVoiceChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<int?> position = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyGuildVoiceChannelAsync(channelID, name, position, isNsfw, bitrate, userLimit, permissionOverwrites, parentID, rtcRegion, videoQualityMode, reason, ct);

    public Task<Result<IChannel>> ModifyGuildStageChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<int?> position = default,
        Optional<int?> bitrate = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<string?> rtcRegion = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyGuildStageChannelAsync(channelID, name, position, bitrate, permissionOverwrites, rtcRegion, reason, ct);

    public Task<Result<IChannel>> ModifyGuildNewsChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<ChannelType> type = default,
        Optional<int?> position = default,
        Optional<string?> topic = default,
        Optional<bool?> isNsfw = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyGuildNewsChannelAsync(channelID, name, type, position, topic, isNsfw, permissionOverwrites, parentID, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> ModifyThreadChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<bool> isArchived = default,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<bool> isLocked = default,
        Optional<bool> isInvitable = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<ChannelFlags> flags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyThreadChannelAsync(channelID, name, isArchived, autoArchiveDuration, isLocked, isInvitable, rateLimitPerUser, flags, reason, ct);

    public Task<Result> DeleteChannelAsync(Snowflake channelID, Optional<string> reason = default, CancellationToken ct = default) => _actual.DeleteChannelAsync(channelID, reason, ct);

    public Task<Result<IMessage>> CreateMessageAsync(Snowflake channelID,
        Optional<string> content = default,
        Optional<string> nonce = default,
        Optional<bool> isTTS = default,
        Optional<IReadOnlyList<IEmbed>> embeds = default,
        Optional<IAllowedMentions> allowedMentions = default,
        Optional<IMessageReference> messageReference = default,
        Optional<IReadOnlyList<IMessageComponent>> components = default,
        Optional<IReadOnlyList<Snowflake>> stickerIDs = default,
        Optional<IReadOnlyList<OneOf<FileData, IPartialAttachment>>> attachments = default,
        Optional<MessageFlags> flags = default,
        CancellationToken ct = default) =>
        _actual.CreateMessageAsync(channelID, content, nonce, isTTS, embeds, allowedMentions, messageReference, components, stickerIDs, attachments, flags, ct);

    public Task<Result<IMessage>> CrosspostMessageAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default) => _actual.CrosspostMessageAsync(channelID, messageID, ct);

    public Task<Result> CreateReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) => _actual.CreateReactionAsync(channelID, messageID, emoji, ct);

    public Task<Result> DeleteOwnReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) =>
        _actual.DeleteOwnReactionAsync(channelID, messageID, emoji, ct);

    public Task<Result> DeleteUserReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, Snowflake user, CancellationToken ct = default) =>
        _actual.DeleteUserReactionAsync(channelID, messageID, emoji, user, ct);

    public Task<Result<IReadOnlyList<IUser>>> GetReactionsAsync(Snowflake channelID,
        Snowflake messageID,
        string emoji,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => _actual.GetReactionsAsync(channelID, messageID, emoji, after, limit, ct);

    public Task<Result> DeleteAllReactionsAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default) => _actual.DeleteAllReactionsAsync(channelID, messageID, ct);

    public Task<Result> DeleteAllReactionsForEmojiAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) =>
        _actual.DeleteAllReactionsForEmojiAsync(channelID, messageID, emoji, ct);

    public Task<Result<IMessage>> EditMessageAsync(Snowflake channelID,
        Snowflake messageID,
        Optional<string?> content = default,
        Optional<IReadOnlyList<IEmbed>?> embeds = default,
        Optional<MessageFlags?> flags = default,
        Optional<IAllowedMentions?> allowedMentions = default,
        Optional<IReadOnlyList<IMessageComponent>?> components = default,
        Optional<IReadOnlyList<OneOf<FileData, IPartialAttachment>>?> attachments = default,
        CancellationToken ct = default) =>
        _actual.EditMessageAsync(channelID, messageID, content, embeds, flags, allowedMentions, components, attachments, ct);

    public Task<Result> DeleteMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.DeleteMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> BulkDeleteMessagesAsync(Snowflake channelID, IReadOnlyList<Snowflake> messageIDs, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.BulkDeleteMessagesAsync(channelID, messageIDs, reason, ct);

    public Task<Result> EditChannelPermissionsAsync(Snowflake channelID,
        Snowflake overwriteID,
        Optional<IDiscordPermissionSet?> allow = default,
        Optional<IDiscordPermissionSet?> deny = default,
        Optional<PermissionOverwriteType> type = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.EditChannelPermissionsAsync(channelID, overwriteID, allow, deny, type, reason, ct);

    public Task<Result<IReadOnlyList<IInvite>>> GetChannelInvitesAsync(Snowflake channelID, CancellationToken ct = default) => _actual.GetChannelInvitesAsync(channelID, ct);

    public Task<Result<IInvite>> CreateChannelInviteAsync(Snowflake channelID,
        Optional<TimeSpan> maxAge = default,
        Optional<int> maxUses = default,
        Optional<bool> isTemporary = default,
        Optional<bool> isUnique = default,
        Optional<InviteTarget> targetType = default,
        Optional<Snowflake> targetUserID = default,
        Optional<Snowflake> targetApplicationID = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.CreateChannelInviteAsync(channelID, maxAge, maxUses, isTemporary, isUnique, targetType, targetUserID, targetApplicationID, reason, ct);

    public Task<Result> DeleteChannelPermissionAsync(Snowflake channelID, Snowflake overwriteID, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.DeleteChannelPermissionAsync(channelID, overwriteID, reason, ct);

    public Task<Result<IFollowedChannel>> FollowNewsChannelAsync(Snowflake channelID, Snowflake webhookChannelID, CancellationToken ct = default) =>
        _actual.FollowNewsChannelAsync(channelID, webhookChannelID, ct);

    public Task<Result> TriggerTypingIndicatorAsync(Snowflake channelID, CancellationToken ct = default) => _actual.TriggerTypingIndicatorAsync(channelID, ct);

    public Task<Result<IReadOnlyList<IMessage>>> GetPinnedMessagesAsync(Snowflake channelID, CancellationToken ct = default) => _actual.GetPinnedMessagesAsync(channelID, ct);

    public Task<Result> PinMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.PinMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> UnpinMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.UnpinMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> GroupDMAddRecipientAsync(Snowflake channelID, Snowflake userID, string accessToken, Optional<string> nickname = default, CancellationToken ct = default) =>
        _actual.GroupDMAddRecipientAsync(channelID, userID, accessToken, nickname, ct);

    public Task<Result> GroupDMRemoveRecipientAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => _actual.GroupDMRemoveRecipientAsync(channelID, userID, ct);

    public Task<Result<IChannel>> StartThreadWithMessageAsync(Snowflake channelID,
        Snowflake messageID,
        string name,
        Optional<AutoArchiveDuration> autoArchiveDuration,
        Optional<int?> rateLimitPerUser = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.StartThreadWithMessageAsync(channelID, messageID, name, autoArchiveDuration, rateLimitPerUser, reason, ct);

    public Task<Result<IChannel>> StartThreadWithoutMessageAsync(Snowflake channelID,
        string name,
        ChannelType type,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<bool> isInvitable = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.StartThreadWithoutMessageAsync(channelID, name, type, autoArchiveDuration, isInvitable, rateLimitPerUser, reason, ct);

    public Task<Result> JoinThreadAsync(Snowflake channelID, CancellationToken ct = default) => _actual.JoinThreadAsync(channelID, ct);

    public Task<Result> AddThreadMemberAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => _actual.AddThreadMemberAsync(channelID, userID, ct);

    public Task<Result> LeaveThreadAsync(Snowflake channelID, CancellationToken ct = default) => _actual.LeaveThreadAsync(channelID, ct);

    public Task<Result> RemoveThreadMemberAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => _actual.RemoveThreadMemberAsync(channelID, userID, ct);

    public Task<Result<IThreadMember>> GetThreadMemberAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => _actual.GetThreadMemberAsync(channelID, userID, ct);

    public Task<Result<IReadOnlyList<IThreadMember>>> ListThreadMembersAsync(Snowflake channelID, CancellationToken ct = default) => _actual.ListThreadMembersAsync(channelID, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListPublicArchivedThreadsAsync(Snowflake channelID,
        Optional<DateTimeOffset> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => _actual.ListPublicArchivedThreadsAsync(channelID, before, limit, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListPrivateArchivedThreadsAsync(Snowflake channelID,
        Optional<DateTimeOffset> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => _actual.ListPrivateArchivedThreadsAsync(channelID, before, limit, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListJoinedPrivateArchivedThreadsAsync(Snowflake channelID,
        Optional<Snowflake> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => _actual.ListJoinedPrivateArchivedThreadsAsync(channelID, before, limit, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (_actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (_actual as IRestCustomizable)?.RemoveCustomization(customization);
}
