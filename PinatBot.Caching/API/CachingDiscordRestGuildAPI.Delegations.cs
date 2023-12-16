using System.Drawing;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestGuildAPI
{
    public Task<Result<IGuild>> CreateGuildAsync(string name,
        Optional<Stream> icon = default,
        Optional<VerificationLevel> verificationLevel = default,
        Optional<MessageNotificationLevel> defaultMessageNotifications = default,
        Optional<ExplicitContentFilterLevel> explicitContentFilter = default,
        Optional<IReadOnlyList<IRole>> roles = default,
        Optional<IReadOnlyList<IPartialChannel>> channels = default,
        Optional<Snowflake> afkChannelID = default,
        Optional<TimeSpan> afkTimeout = default,
        Optional<Snowflake> systemChannelID = default,
        Optional<SystemChannelFlags> systemChannelFlags = default,
        CancellationToken ct = default) =>
        actual.CreateGuildAsync(name, icon, verificationLevel, defaultMessageNotifications, explicitContentFilter, roles, channels, afkChannelID, afkTimeout, systemChannelID, systemChannelFlags, ct);

    public Task<Result<IGuildPreview>> GetGuildPreviewAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildPreviewAsync(guildID, ct);

    public Task<Result<IGuild>> ModifyGuildAsync(Snowflake guildID,
        Optional<string> name = default,
        Optional<VerificationLevel?> verificationLevel = default,
        Optional<MessageNotificationLevel?> defaultMessageNotifications = default,
        Optional<ExplicitContentFilterLevel?> explicitContentFilter = default,
        Optional<Snowflake?> afkChannelID = default,
        Optional<TimeSpan> afkTimeout = default,
        Optional<Stream?> icon = default,
        Optional<Snowflake> ownerID = default,
        Optional<Stream?> splash = default,
        Optional<Stream?> discoverySplash = default,
        Optional<Stream?> banner = default,
        Optional<Snowflake?> systemChannelID = default,
        Optional<SystemChannelFlags> systemChannelFlags = default,
        Optional<Snowflake?> rulesChannelID = default,
        Optional<Snowflake?> publicUpdatesChannelID = default,
        Optional<string?> preferredLocale = default,
        Optional<IReadOnlyList<GuildFeature>> features = default,
        Optional<string?> description = default,
        Optional<bool> isPremiumProgressBarEnabled = default,
        Optional<Snowflake?> safetyAlertsChannelID = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildAsync(guildID, name, verificationLevel, defaultMessageNotifications, explicitContentFilter, afkChannelID, afkTimeout, icon, ownerID, splash, discoverySplash, banner,
            systemChannelID, systemChannelFlags, rulesChannelID, publicUpdatesChannelID, preferredLocale, features, description, isPremiumProgressBarEnabled, safetyAlertsChannelID, reason, ct);

    public Task<Result> DeleteGuildAsync(Snowflake guildID, CancellationToken ct = default) => actual.DeleteGuildAsync(guildID, ct);

    public Task<Result<IChannel>> CreateGuildChannelAsync(Snowflake guildID,
        string name,
        Optional<ChannelType?> type = default,
        Optional<string?> topic = default,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<IDefaultReaction?> defaultReactionEmoji = default,
        Optional<IReadOnlyList<IForumTag>?> availableTags = default,
        Optional<SortOrder?> defaultSortOrder = default,
        Optional<ForumLayout?> defaultForumLayout = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildChannelAsync(guildID, name, type, topic, bitrate, userLimit, rateLimitPerUser, position, permissionOverwrites, parentID, isNsfw, rtcRegion, videoQualityMode,
            defaultAutoArchiveDuration, defaultReactionEmoji, availableTags, defaultSortOrder, defaultForumLayout, reason, ct);

    public Task<Result<IChannel>> CreateGuildTextChannelAsync(Snowflake guildID,
        string name,
        Optional<string?> topic = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildTextChannelAsync(guildID, name, topic, rateLimitPerUser, position, permissionOverwrites, parentID, isNsfw, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> CreateGuildAnnouncementChannelAsync(Snowflake guildID,
        string name,
        Optional<int?> bitrate = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildAnnouncementChannelAsync(guildID, name, bitrate, position, permissionOverwrites, parentID, isNsfw, defaultAutoArchiveDuration, reason, ct);

    public Task<Result<IChannel>> CreateGuildForumChannelAsync(Snowflake guildID,
        string name,
        Optional<string?> topic = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<AutoArchiveDuration?> defaultAutoArchiveDuration = default,
        Optional<IDefaultReaction?> defaultReactionEmoji = default,
        Optional<IReadOnlyList<IForumTag>?> availableTags = default,
        Optional<SortOrder?> defaultSortOrder = default,
        Optional<ForumLayout?> defaultForumLayout = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildForumChannelAsync(guildID, name, topic, position, permissionOverwrites, parentID, isNsfw, defaultAutoArchiveDuration, defaultReactionEmoji, availableTags, defaultSortOrder,
            defaultForumLayout, reason, ct);

    public Task<Result<IChannel>> CreateGuildVoiceChannelAsync(Snowflake guildID,
        string name,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildVoiceChannelAsync(guildID, name, bitrate, userLimit, rateLimitPerUser, position, permissionOverwrites, parentID, isNsfw, rtcRegion, videoQualityMode, reason, ct);

    public Task<Result<IChannel>> CreateGuildStageChannelAsync(Snowflake guildID,
        string name,
        Optional<int?> bitrate = default,
        Optional<int?> userLimit = default,
        Optional<int?> rateLimitPerUser = default,
        Optional<int?> position = default,
        Optional<IReadOnlyList<IPartialPermissionOverwrite>?> permissionOverwrites = default,
        Optional<Snowflake?> parentID = default,
        Optional<bool?> isNsfw = default,
        Optional<string?> rtcRegion = default,
        Optional<VideoQualityMode?> videoQualityMode = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildStageChannelAsync(guildID, name, bitrate, userLimit, rateLimitPerUser, position, permissionOverwrites, parentID, isNsfw, rtcRegion, videoQualityMode, reason, ct);

    public Task<Result> ModifyGuildChannelPositionsAsync(Snowflake guildID,
        IReadOnlyList<(Snowflake ChannelID, int? Position, bool? LockPermissions, Snowflake? ParentID)> positionModifications,
        CancellationToken ct = default) => actual.ModifyGuildChannelPositionsAsync(guildID, positionModifications, ct);

    public Task<Result<IGuildThreadQueryResponse>> ListActiveGuildThreadsAsync(Snowflake guildID, CancellationToken ct = default) => actual.ListActiveGuildThreadsAsync(guildID, ct);

    public Task<Result<IGuildMember?>> AddGuildMemberAsync(Snowflake guildID,
        Snowflake userID,
        string accessToken,
        Optional<string> nickname = default,
        Optional<IReadOnlyList<Snowflake>> roles = default,
        Optional<bool> isMuted = default,
        Optional<bool> isDeafened = default,
        CancellationToken ct = default) =>
        actual.AddGuildMemberAsync(guildID, userID, accessToken, nickname, roles, isMuted, isDeafened, ct);

    public Task<Result> ModifyGuildMemberAsync(Snowflake guildID,
        Snowflake userID,
        Optional<string?> nickname = default,
        Optional<IReadOnlyList<Snowflake>?> roles = default,
        Optional<bool?> isMuted = default,
        Optional<bool?> isDeafened = default,
        Optional<Snowflake?> channelID = default,
        Optional<DateTimeOffset?> communicationDisabledUntil = default,
        Optional<GuildMemberFlags> flags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildMemberAsync(guildID, userID, nickname, roles, isMuted, isDeafened, channelID, communicationDisabledUntil, flags, reason, ct);

    public Task<Result<IGuildMember>> ModifyCurrentMemberAsync(Snowflake guildID, Optional<string?> nickname = default, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.ModifyCurrentMemberAsync(guildID, nickname, reason, ct);

    public Task<Result> AddGuildMemberRoleAsync(Snowflake guildID, Snowflake userID, Snowflake roleID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.AddGuildMemberRoleAsync(guildID, userID, roleID, reason, ct);

    public Task<Result> RemoveGuildMemberRoleAsync(Snowflake guildID, Snowflake userID, Snowflake roleID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.RemoveGuildMemberRoleAsync(guildID, userID, roleID, reason, ct);

    public Task<Result> RemoveGuildMemberAsync(Snowflake guildID, Snowflake userID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.RemoveGuildMemberAsync(guildID, userID, reason, ct);

    public Task<Result<IReadOnlyList<IBan>>> GetGuildBansAsync(Snowflake guildID,
        Optional<int> limit = default,
        Optional<Snowflake> before = default,
        Optional<Snowflake> after = default,
        CancellationToken ct = default) => actual.GetGuildBansAsync(guildID, limit, before, after, ct);

    public Task<Result<IBan>> GetGuildBanAsync(Snowflake guildID, Snowflake userID, CancellationToken ct = default) => actual.GetGuildBanAsync(guildID, userID, ct);

    public Task<Result> CreateGuildBanAsync(Snowflake guildID, Snowflake userID, Optional<int> deleteMessageDays = default, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.CreateGuildBanAsync(guildID, userID, deleteMessageDays, reason, ct);

    public Task<Result> RemoveGuildBanAsync(Snowflake guildID, Snowflake userID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.RemoveGuildBanAsync(guildID, userID, reason, ct);

    public Task<Result<IRole>> CreateGuildRoleAsync(Snowflake guildID,
        Optional<string> name = default,
        Optional<IDiscordPermissionSet> permissions = default,
        Optional<Color> colour = default,
        Optional<bool> isHoisted = default,
        Optional<Stream?> icon = default,
        Optional<string?> unicodeEmoji = default,
        Optional<bool> isMentionable = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.CreateGuildRoleAsync(guildID, name, permissions, colour, isHoisted, icon, unicodeEmoji, isMentionable, reason, ct);

    public Task<Result<IReadOnlyList<IRole>>> ModifyGuildRolePositionsAsync(Snowflake guildID,
        IReadOnlyList<(Snowflake RoleID, Optional<int?> Position)> modifiedPositions,
        Optional<string> reason = default,
        CancellationToken ct = default) => actual.ModifyGuildRolePositionsAsync(guildID, modifiedPositions, reason, ct);

    public Task<Result<IRole>> ModifyGuildRoleAsync(Snowflake guildID,
        Snowflake roleID,
        Optional<string?> name = default,
        Optional<IDiscordPermissionSet?> permissions = default,
        Optional<Color?> color = default,
        Optional<bool?> isHoisted = default,
        Optional<Stream?> icon = default,
        Optional<string?> unicodeEmoji = default,
        Optional<bool?> isMentionable = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildRoleAsync(guildID, roleID, name, permissions, color, isHoisted, icon, unicodeEmoji, isMentionable, reason, ct);

    public Task<Result<MultiFactorAuthenticationLevel>> ModifyGuildMFALevelAsync(Snowflake guildID,
        MultiFactorAuthenticationLevel level,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildMFALevelAsync(guildID, level, reason, ct);

    public Task<Result> DeleteGuildRoleAsync(Snowflake guildID, Snowflake roleID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.DeleteGuildRoleAsync(guildID, roleID, reason, ct);

    public Task<Result<IPruneCount>>
        GetGuildPruneCountAsync(Snowflake guildID, Optional<int> days = default, Optional<IReadOnlyList<Snowflake>> includeRoles = default, CancellationToken ct = default) =>
        actual.GetGuildPruneCountAsync(guildID, days, includeRoles, ct);

    public Task<Result<IPruneCount>> BeginGuildPruneAsync(Snowflake guildID,
        Optional<int> days = default,
        Optional<bool> computePruneCount = default,
        Optional<IReadOnlyList<Snowflake>> includeRoles = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.BeginGuildPruneAsync(guildID, days, computePruneCount, includeRoles, reason, ct);

    public Task<Result<IReadOnlyList<IVoiceRegion>>> GetGuildVoiceRegionsAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildVoiceRegionsAsync(guildID, ct);

    public Task<Result<IReadOnlyList<IInvite>>> GetGuildInvitesAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildInvitesAsync(guildID, ct);

    public Task<Result<IReadOnlyList<IIntegration>>> GetGuildIntegrationsAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildIntegrationsAsync(guildID, ct);

    public Task<Result> DeleteGuildIntegrationAsync(Snowflake guildID, Snowflake integrationID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.DeleteGuildIntegrationAsync(guildID, integrationID, reason, ct);

    public Task<Result<IGuildWidgetSettings>> GetGuildWidgetSettingsAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildWidgetSettingsAsync(guildID, ct);

    public Task<Result<IGuildWidgetSettings>> ModifyGuildWidgetAsync(Snowflake guildID,
        Optional<bool> isEnabled = default,
        Optional<Snowflake?> channelID = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildWidgetAsync(guildID, isEnabled, channelID, reason, ct);

    public Task<Result<IGuildWidget>> GetGuildWidgetAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildWidgetAsync(guildID, ct);

    public Task<Result<IPartialInvite>> GetGuildVanityUrlAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildVanityUrlAsync(guildID, ct);

    public Task<Result<Stream>> GetGuildWidgetImageAsync(Snowflake guildID, Optional<WidgetImageStyle> style = default, CancellationToken ct = default) =>
        actual.GetGuildWidgetImageAsync(guildID, style, ct);

    public Task<Result<IWelcomeScreen>> GetGuildWelcomeScreenAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildWelcomeScreenAsync(guildID, ct);

    public Task<Result<IWelcomeScreen>> ModifyGuildWelcomeScreenAsync(Snowflake guildID,
        Optional<bool?> isEnabled = default,
        Optional<IReadOnlyList<IWelcomeScreenChannel>?> welcomeChannels = default,
        Optional<string?> description = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildWelcomeScreenAsync(guildID, isEnabled, welcomeChannels, description, reason, ct);

    public Task<Result<IGuildOnboarding>> GetGuildOnboardingAsync(Snowflake guildID, CancellationToken ct = default) => actual.GetGuildOnboardingAsync(guildID, ct);

    public Task<Result<IGuildOnboarding>> ModifyGuildOnboardingAsync(Snowflake guildID,
        IReadOnlyList<IOnboardingPrompt> prompts,
        IReadOnlyList<Snowflake> defaultChannelIDs,
        bool isEnabled,
        GuildOnboardingMode mode,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildOnboardingAsync(guildID, prompts, defaultChannelIDs, isEnabled, mode, reason, ct);

    public Task<Result> ModifyCurrentUserVoiceStateAsync(Snowflake guildID,
        Snowflake channelID,
        Optional<bool> suppress = default,
        Optional<DateTimeOffset?> requestToSpeakTimestamp = default,
        CancellationToken ct = default) =>
        actual.ModifyCurrentUserVoiceStateAsync(guildID, channelID, suppress, requestToSpeakTimestamp, ct);

    public Task<Result<IVoiceState>> ModifyUserVoiceStateAsync(Snowflake guildID,
        Snowflake userID,
        Snowflake? channelID = default,
        Optional<bool> suppress = default,
        CancellationToken ct = default) =>
        actual.ModifyUserVoiceStateAsync(guildID, userID, channelID, suppress, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (actual as IRestCustomizable)?.RemoveCustomization(customization);
}
