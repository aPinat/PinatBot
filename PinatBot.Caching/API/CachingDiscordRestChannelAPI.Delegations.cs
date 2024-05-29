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
        Optional<IReadOnlyList<IPartialForumTag>> availableTags = default,
        Optional<IDefaultReaction?> defaultReactionEmoji = default,
        Optional<int> defaultThreadRateLimitPerUser = default,
        Optional<IReadOnlyList<Snowflake>> appliedTags = default,
        Optional<SortOrder> defaultSortOrder = default,
        Optional<ForumLayout> defaultForumLayout = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyChannelAsync(channelID, name, icon, type, position, topic, isNsfw, rateLimitPerUser, bitrate, userLimit, permissionOverwrites, parentID, videoQualityMode, isArchived,
            autoArchiveDuration, isLocked, isInvitable, defaultAutoArchiveDuration, rtcRegion, flags, availableTags, defaultReactionEmoji, defaultThreadRateLimitPerUser, appliedTags, defaultSortOrder,
            defaultForumLayout, reason, ct);

    public Task<Result<IChannel>> ModifyGroupDMChannelAsync(Snowflake channelID, Optional<string> name = default, Optional<Stream> icon = default, CancellationToken ct = default) =>
        actual.ModifyGroupDMChannelAsync(channelID, name, icon, ct);

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
        actual.ModifyGuildTextChannelAsync(channelID, name, type, position, topic, isNsfw, rateLimitPerUser, permissionOverwrites, parentID, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> ModifyGuildVoiceChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<int?> position = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildVoiceChannelAsync(channelID, name, position, isNsfw, rateLimitPerUser, bitrate, userLimit, permissionOverwrites, parentID, rtcRegion, videoQualityMode, reason, ct);

    public Task<Result<IChannel>> ModifyGuildStageChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<int?> position = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildStageChannelAsync(channelID, name, position, isNsfw, rateLimitPerUser, bitrate, userLimit, permissionOverwrites, parentID, rtcRegion, videoQualityMode, reason, ct);

    public Task<Result<IChannel>> ModifyGuildAnnouncementChannelAsync(Snowflake channelID,
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
        actual.ModifyGuildAnnouncementChannelAsync(channelID, name, type, position, topic, isNsfw, permissionOverwrites, parentID, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> ModifyThreadChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<bool> isArchived = default,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<bool> isLocked = default,
        Optional<bool> isInvitable = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<ChannelFlags> flags = default,
        Optional<IReadOnlyList<Snowflake>> appliedTags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyThreadChannelAsync(channelID, name, isArchived, autoArchiveDuration, isLocked, isInvitable, rateLimitPerUser, flags, appliedTags, reason, ct);

    public Task<Result<IChannel>> ModifyForumChannelAsync(Snowflake channelID,
        Optional<string> name = default,
        Optional<int?> position = default,
        Optional<string?> topic = default,
        Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<ChannelFlags> flags = default,
        Optional<IReadOnlyList<IPartialForumTag>> availableTags = default,
        Optional<IDefaultReaction?> defaultReactionEmoji = default,
        Optional<int> defaultThreadRateLimitPerUser = default,
        Optional<SortOrder> defaultSortOrder = default,
        Optional<ForumLayout> defaultForumLayout = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyForumChannelAsync(channelID, name, position, topic, isNsfw, rateLimitPerUser, permissionOverwrites, parentID, defaultAutoArchiveDuration, flags, availableTags,
            defaultReactionEmoji, defaultThreadRateLimitPerUser, defaultSortOrder, defaultForumLayout, reason, ct);

    public Task<Result<IChannel>> ModifyMediaChannelAsync(Snowflake channelID, Optional<string> name = default, Optional<int?> position = default,
        Optional<string?> topic = default, Optional<bool?> isNsfw = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default, Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<ChannelFlags> flags = default, Optional<IReadOnlyList<IPartialForumTag>> availableTags = default,
        Optional<IDefaultReaction?> defaultReactionEmoji = default, Optional<int> defaultThreadRateLimitPerUser = default,
        Optional<SortOrder> defaultSortOrder = default, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.ModifyMediaChannelAsync(channelID, name, position, topic, isNsfw, rateLimitPerUser, permissionOverwrites, parentID, defaultAutoArchiveDuration, flags, availableTags,
            defaultReactionEmoji, defaultThreadRateLimitPerUser, defaultSortOrder, reason, ct);

    public Task<Result> DeleteChannelAsync(Snowflake channelID, Optional<string> reason = default, CancellationToken ct = default) => actual.DeleteChannelAsync(channelID, reason, ct);

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
        Optional<bool> enforceNonce = default,
        Optional<IPollCreateRequest> poll = default,
        CancellationToken ct = default) =>
        actual.CreateMessageAsync(channelID, content, nonce, isTTS, embeds, allowedMentions, messageReference, components, stickerIDs, attachments, flags, enforceNonce, poll, ct);

    public Task<Result<IMessage>> CrosspostMessageAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default) => actual.CrosspostMessageAsync(channelID, messageID, ct);

    public Task<Result> CreateReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) => actual.CreateReactionAsync(channelID, messageID, emoji, ct);

    public Task<Result> DeleteOwnReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) =>
        actual.DeleteOwnReactionAsync(channelID, messageID, emoji, ct);

    public Task<Result> DeleteUserReactionAsync(Snowflake channelID, Snowflake messageID, string emoji, Snowflake user, CancellationToken ct = default) =>
        actual.DeleteUserReactionAsync(channelID, messageID, emoji, user, ct);

    public Task<Result<IReadOnlyList<IUser>>> GetReactionsAsync(Snowflake channelID,
        Snowflake messageID,
        string emoji,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => actual.GetReactionsAsync(channelID, messageID, emoji, after, limit, ct);

    public Task<Result> DeleteAllReactionsAsync(Snowflake channelID, Snowflake messageID, CancellationToken ct = default) => actual.DeleteAllReactionsAsync(channelID, messageID, ct);

    public Task<Result> DeleteAllReactionsForEmojiAsync(Snowflake channelID, Snowflake messageID, string emoji, CancellationToken ct = default) =>
        actual.DeleteAllReactionsForEmojiAsync(channelID, messageID, emoji, ct);

    public Task<Result<IMessage>> EditMessageAsync(Snowflake channelID,
        Snowflake messageID,
        Optional<string?> content = default,
        Optional<IReadOnlyList<IEmbed>?> embeds = default,
        Optional<MessageFlags?> flags = default,
        Optional<IAllowedMentions?> allowedMentions = default,
        Optional<IReadOnlyList<IMessageComponent>?> components = default,
        Optional<IReadOnlyList<OneOf<FileData, IPartialAttachment>>?> attachments = default,
        CancellationToken ct = default) =>
        actual.EditMessageAsync(channelID, messageID, content, embeds, flags, allowedMentions, components, attachments, ct);

    public Task<Result> DeleteMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.DeleteMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> BulkDeleteMessagesAsync(Snowflake channelID, IReadOnlyList<Snowflake> messageIDs, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.BulkDeleteMessagesAsync(channelID, messageIDs, reason, ct);

    public Task<Result> EditChannelPermissionsAsync(Snowflake channelID,
        Snowflake overwriteID,
        Optional<IDiscordPermissionSet?> allow = default,
        Optional<IDiscordPermissionSet?> deny = default,
        Optional<PermissionOverwriteType> type = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.EditChannelPermissionsAsync(channelID, overwriteID, allow, deny, type, reason, ct);

    public Task<Result<IReadOnlyList<IInvite>>> GetChannelInvitesAsync(Snowflake channelID, CancellationToken ct = default) => actual.GetChannelInvitesAsync(channelID, ct);

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
        actual.CreateChannelInviteAsync(channelID, maxAge, maxUses, isTemporary, isUnique, targetType, targetUserID, targetApplicationID, reason, ct);

    public Task<Result> DeleteChannelPermissionAsync(Snowflake channelID, Snowflake overwriteID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.DeleteChannelPermissionAsync(channelID, overwriteID, reason, ct);

    public Task<Result<IFollowedChannel>> FollowAnnouncementChannelAsync(Snowflake channelID, Snowflake webhookChannelID, CancellationToken ct = default) =>
        actual.FollowAnnouncementChannelAsync(channelID, webhookChannelID, ct);

    public Task<Result> TriggerTypingIndicatorAsync(Snowflake channelID, CancellationToken ct = default) => actual.TriggerTypingIndicatorAsync(channelID, ct);

    public Task<Result<IReadOnlyList<IMessage>>> GetPinnedMessagesAsync(Snowflake channelID, CancellationToken ct = default) => actual.GetPinnedMessagesAsync(channelID, ct);

    public Task<Result> PinMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.PinMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> UnpinMessageAsync(Snowflake channelID, Snowflake messageID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.UnpinMessageAsync(channelID, messageID, reason, ct);

    public Task<Result> GroupDMAddRecipientAsync(Snowflake channelID, Snowflake userID, string accessToken, Optional<string> nickname = default, CancellationToken ct = default) =>
        actual.GroupDMAddRecipientAsync(channelID, userID, accessToken, nickname, ct);

    public Task<Result> GroupDMRemoveRecipientAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => actual.GroupDMRemoveRecipientAsync(channelID, userID, ct);

    public Task<Result<IChannel>> StartThreadFromMessageAsync(Snowflake channelID,
        Snowflake messageID,
        string name,
        Optional<AutoArchiveDuration> autoArchiveDuration,
        Optional<int?> rateLimitPerUser = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.StartThreadFromMessageAsync(channelID, messageID, name, autoArchiveDuration, rateLimitPerUser, reason, ct);

    public Task<Result<IChannel>> StartThreadWithoutMessageAsync(Snowflake channelID,
        string name,
        ChannelType type,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<bool> isInvitable = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.StartThreadWithoutMessageAsync(channelID, name, type, autoArchiveDuration, isInvitable, rateLimitPerUser, reason, ct);

    public Task<Result<IChannel>> StartThreadInForumChannelAsync(Snowflake channelID,
        string name,
        Optional<AutoArchiveDuration> autoArchiveDuration = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<string> content = default,
        Optional<IReadOnlyList<IEmbed>> embeds = default,
        Optional<IAllowedMentions> allowedMentions = default,
        Optional<IReadOnlyList<IMessageComponent>> components = default,
        Optional<IReadOnlyList<Snowflake>> stickerIds = default,
        Optional<IReadOnlyList<FileData>> attachments = default,
        Optional<MessageFlags> flags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.StartThreadInForumChannelAsync(channelID, name, autoArchiveDuration, rateLimitPerUser, content, embeds, allowedMentions, components, stickerIds, attachments, flags, reason, ct);

    public Task<Result> JoinThreadAsync(Snowflake channelID, CancellationToken ct = default) => actual.JoinThreadAsync(channelID, ct);

    public Task<Result> AddThreadMemberAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => actual.AddThreadMemberAsync(channelID, userID, ct);

    public Task<Result> LeaveThreadAsync(Snowflake channelID, CancellationToken ct = default) => actual.LeaveThreadAsync(channelID, ct);

    public Task<Result> RemoveThreadMemberAsync(Snowflake channelID, Snowflake userID, CancellationToken ct = default) => actual.RemoveThreadMemberAsync(channelID, userID, ct);

    public Task<Result<IThreadMember>> GetThreadMemberAsync(Snowflake channelID, Snowflake userID, Optional<bool> withMember = default, CancellationToken ct = default) =>
        actual.GetThreadMemberAsync(channelID, userID, withMember, ct);

    public Task<Result<IReadOnlyList<IThreadMember>>> ListThreadMembersAsync(Snowflake channelID,
        Optional<bool> withMember = default,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default) =>
        actual.ListThreadMembersAsync(channelID, withMember, after, limit, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListPublicArchivedThreadsAsync(Snowflake channelID,
        Optional<DateTimeOffset> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => actual.ListPublicArchivedThreadsAsync(channelID, before, limit, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListPrivateArchivedThreadsAsync(Snowflake channelID,
        Optional<DateTimeOffset> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => actual.ListPrivateArchivedThreadsAsync(channelID, before, limit, ct);

    public Task<Result<IChannelThreadQueryResponse>> ListJoinedPrivateArchivedThreadsAsync(Snowflake channelID,
        Optional<Snowflake> before = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => actual.ListJoinedPrivateArchivedThreadsAsync(channelID, before, limit, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (actual as IRestCustomizable)?.RemoveCustomization(customization);
}
