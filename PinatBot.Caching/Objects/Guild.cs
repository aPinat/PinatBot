using System.Collections.Concurrent;
using System.Collections.Immutable;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;

namespace PinatBot.Caching.Objects;

public class Guild : IGuildCreate
{
    public Guild(Snowflake id) => ID = id;

    public Snowflake ID { get; }

    public string Name { get; private set; } = null!;

    public IImageHash? Icon { get; private set; }

    public IImageHash? Splash { get; private set; }

    public IImageHash? DiscoverySplash { get; private set; }

    public Optional<bool> IsOwner { get; private set; }

    public Snowflake OwnerID { get; private set; }

    public Optional<IDiscordPermissionSet> Permissions { get; private set; }

    public Snowflake? AFKChannelID { get; private set; }

    public TimeSpan AFKTimeout { get; private set; }

    public VerificationLevel VerificationLevel { get; private set; }

    public MessageNotificationLevel DefaultMessageNotifications { get; private set; }

    public ExplicitContentFilterLevel ExplicitContentFilter { get; private set; }

    internal readonly ConcurrentDictionary<ulong, IRole> RolesInternal = new();
    public IReadOnlyList<IRole> Roles => RolesInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IEmoji> EmojisInternal = new();
    public IReadOnlyList<IEmoji> Emojis => EmojisInternal.Values.ToImmutableList();

    public IReadOnlyList<GuildFeature> GuildFeatures { get; private set; } = null!;

    public MultiFactorAuthenticationLevel MFALevel { get; private set; }

    public Snowflake? ApplicationID { get; private set; }

    public Optional<bool> IsWidgetEnabled { get; private set; }

    public Optional<Snowflake?> WidgetChannelID { get; private set; }

    public Snowflake? SystemChannelID { get; private set; }

    public SystemChannelFlags SystemChannelFlags { get; private set; }

    public Snowflake? RulesChannelID { get; private set; }

    public Optional<int?> MaxPresences { get; private set; }

    public Optional<int> MaxMembers { get; private set; }

    public string? VanityUrlCode { get; private set; }

    public string? Description { get; private set; }

    public IImageHash? Banner { get; private set; }

    public PremiumTier PremiumTier { get; private set; }

    public Optional<int> PremiumSubscriptionCount { get; private set; }

    public string PreferredLocale { get; private set; } = null!;

    public Snowflake? PublicUpdatesChannelID { get; private set; }

    public Optional<int> MaxVideoChannelUsers { get; private set; }

    public Optional<int> ApproximateMemberCount { get; private set; }

    public Optional<int> ApproximatePresenceCount { get; private set; }

    public Optional<IWelcomeScreen> WelcomeScreen { get; private set; }

    public GuildNSFWLevel NSFWLevel { get; private set; }

    internal readonly ConcurrentDictionary<ulong, ISticker> StickersInternal = new();
    public Optional<IReadOnlyList<ISticker>> Stickers => StickersInternal.Values.ToImmutableList();

    public bool IsPremiumProgressBarEnabled { get; private set; }

    public Optional<DateTimeOffset> JoinedAt { get; private set; }

    public Optional<bool> IsLarge { get; private set; }

    public Optional<bool> IsUnavailable { get; internal set; }

    public Optional<int> MemberCount { get; internal set; }

    internal readonly ConcurrentDictionary<ulong, IPartialVoiceState> VoiceStatesInternal = new();
    public Optional<IReadOnlyList<IPartialVoiceState>> VoiceStates => VoiceStatesInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IGuildMember> MembersInternal = new();
    public Optional<IReadOnlyList<IGuildMember>> Members => MembersInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IChannel> ChannelsInternal = new();
    public Optional<IReadOnlyList<IChannel>> Channels => ChannelsInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IChannel> ThreadsInternal = new();
    public Optional<IReadOnlyList<IChannel>> Threads => ThreadsInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IPartialPresence> PresencesInternal = new();
    public Optional<IReadOnlyList<IPartialPresence>> Presences => PresencesInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IStageInstance> StageInstancesInternal = new();
    public Optional<IReadOnlyList<IStageInstance>> StageInstances => StageInstancesInternal.Values.ToImmutableList();

    internal readonly ConcurrentDictionary<ulong, IGuildScheduledEvent> GuildScheduledEventsInternal = new();
    public Optional<IReadOnlyList<IGuildScheduledEvent>> GuildScheduledEvents => GuildScheduledEventsInternal.Values.ToImmutableList();

    internal void Populate(IGuildCreate g)
    {
        if (g.ID != ID)
            throw new InvalidOperationException("Guild ID mismatch");

        Name = g.Name;
        Icon = g.Icon;
        Splash = g.Splash;
        DiscoverySplash = g.DiscoverySplash;
        IsOwner = g.IsOwner;
        OwnerID = g.OwnerID;
        Permissions = g.Permissions;
        AFKChannelID = g.AFKChannelID;
        AFKTimeout = g.AFKTimeout;
        VerificationLevel = g.VerificationLevel;
        DefaultMessageNotifications = g.DefaultMessageNotifications;
        ExplicitContentFilter = g.ExplicitContentFilter;

        RolesInternal.Clear();
        foreach (var role in g.Roles)
            RolesInternal[role.ID.Value] = role;

        EmojisInternal.Clear();
        foreach (var emoji in g.Emojis)
            if (emoji.ID is { } id)
                EmojisInternal[id.Value] = emoji;

        GuildFeatures = g.GuildFeatures;
        MFALevel = g.MFALevel;
        ApplicationID = g.ApplicationID;
        IsWidgetEnabled = g.IsWidgetEnabled;
        WidgetChannelID = g.WidgetChannelID;
        SystemChannelID = g.SystemChannelID;
        SystemChannelFlags = g.SystemChannelFlags;
        RulesChannelID = g.RulesChannelID;
        MaxPresences = g.MaxPresences;
        MaxMembers = g.MaxMembers;
        VanityUrlCode = g.VanityUrlCode;
        Description = g.Description;
        Banner = g.Banner;
        PremiumTier = g.PremiumTier;
        PremiumSubscriptionCount = g.PremiumSubscriptionCount;
        PreferredLocale = g.PreferredLocale;
        PublicUpdatesChannelID = g.PublicUpdatesChannelID;
        MaxVideoChannelUsers = g.MaxVideoChannelUsers;
        ApproximateMemberCount = g.ApproximateMemberCount;
        ApproximatePresenceCount = g.ApproximatePresenceCount;
        WelcomeScreen = g.WelcomeScreen;
        NSFWLevel = g.NSFWLevel;

        StickersInternal.Clear();
        if (g.Stickers.IsDefined(out var stickers))
            foreach (var sticker in stickers)
                StickersInternal[sticker.ID.Value] = sticker;

        IsPremiumProgressBarEnabled = g.IsPremiumProgressBarEnabled;

        JoinedAt = g.JoinedAt;
        IsLarge = g.IsLarge;
        IsUnavailable = g.IsUnavailable;
        MemberCount = g.MemberCount;

        VoiceStatesInternal.Clear();
        if (g.VoiceStates.IsDefined(out var voiceStates))
            foreach (var voiceState in voiceStates)
                if (voiceState.UserID.IsDefined(out var userID))
                    VoiceStatesInternal[userID.Value] = voiceState;

        MembersInternal.Clear();
        if (g.Members.IsDefined(out var members))
            foreach (var member in members)
                if (member.User.IsDefined(out var user))
                    MembersInternal[user.ID.Value] = member;

        ChannelsInternal.Clear();
        if (g.Channels.IsDefined(out var channels))
            foreach (var channel in channels)
                ChannelsInternal[channel.ID.Value] = (channel as Channel ?? throw new InvalidCastException()) with { GuildID = ID };

        ThreadsInternal.Clear();
        if (g.Threads.IsDefined(out var threads))
            foreach (var thread in threads)
                ThreadsInternal[thread.ID.Value] = (thread as Channel ?? throw new InvalidCastException()) with { GuildID = ID };

        PresencesInternal.Clear();
        if (g.Presences.IsDefined(out var presences))
            foreach (var presence in presences)
                if (presence.User.IsDefined(out var user) && user.ID.IsDefined(out var userID))
                    PresencesInternal[userID.Value] = presence;

        StageInstancesInternal.Clear();
        if (g.StageInstances.IsDefined(out var stageInstances))
            foreach (var stageInstance in stageInstances)
                StageInstancesInternal[stageInstance.ID.Value] = stageInstance;

        GuildScheduledEventsInternal.Clear();
        if (g.GuildScheduledEvents.IsDefined(out var guildScheduledEvents))
            foreach (var guildScheduledEvent in guildScheduledEvents)
                GuildScheduledEventsInternal[guildScheduledEvent.ID.Value] = guildScheduledEvent;
    }

    internal void Update(IGuildUpdate g)
    {
        if (g.ID != ID)
            throw new InvalidOperationException("Guild ID mismatch");

        Name = g.Name;
        Icon = g.Icon;
        Splash = g.Splash;
        DiscoverySplash = g.DiscoverySplash;
        IsOwner = g.IsOwner;
        OwnerID = g.OwnerID;
        Permissions = g.Permissions;
        AFKChannelID = g.AFKChannelID;
        AFKTimeout = g.AFKTimeout;
        VerificationLevel = g.VerificationLevel;
        DefaultMessageNotifications = g.DefaultMessageNotifications;
        ExplicitContentFilter = g.ExplicitContentFilter;

        RolesInternal.Clear();
        foreach (var role in g.Roles)
            RolesInternal[role.ID.Value] = role;

        EmojisInternal.Clear();
        foreach (var emoji in g.Emojis)
            if (emoji.ID is { } id)
                EmojisInternal[id.Value] = emoji;

        GuildFeatures = g.GuildFeatures;
        MFALevel = g.MFALevel;
        ApplicationID = g.ApplicationID;
        IsWidgetEnabled = g.IsWidgetEnabled;
        WidgetChannelID = g.WidgetChannelID;
        SystemChannelID = g.SystemChannelID;
        SystemChannelFlags = g.SystemChannelFlags;
        RulesChannelID = g.RulesChannelID;
        MaxPresences = g.MaxPresences;
        MaxMembers = g.MaxMembers;
        VanityUrlCode = g.VanityUrlCode;
        Description = g.Description;
        Banner = g.Banner;
        PremiumTier = g.PremiumTier;
        PremiumSubscriptionCount = g.PremiumSubscriptionCount;
        PreferredLocale = g.PreferredLocale;
        PublicUpdatesChannelID = g.PublicUpdatesChannelID;
        MaxVideoChannelUsers = g.MaxVideoChannelUsers;
        ApproximateMemberCount = g.ApproximateMemberCount;
        ApproximatePresenceCount = g.ApproximatePresenceCount;
        WelcomeScreen = g.WelcomeScreen;
        NSFWLevel = g.NSFWLevel;

        StickersInternal.Clear();
        if (g.Stickers.IsDefined(out var stickers))
            foreach (var sticker in stickers)
                StickersInternal[sticker.ID.Value] = sticker;

        IsPremiumProgressBarEnabled = g.IsPremiumProgressBarEnabled;
    }

    internal void Update(IPresenceUpdate p, Snowflake userId) => PresencesInternal[userId.Value] = p;
}
