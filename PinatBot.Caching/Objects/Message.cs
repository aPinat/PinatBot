using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;

namespace PinatBot.Caching.Objects;

public class Message(Snowflake id, Snowflake channelID) : IMessageCreate
{
    public Snowflake ID { get; } = id;

    public Snowflake ChannelID { get; } = channelID;

    public Optional<Snowflake> GuildID { get; private set; }

    public IUser Author { get; private set; } = null!;

    public Optional<IPartialGuildMember> Member { get; private set; }

    public string Content { get; private set; } = null!;

    public DateTimeOffset Timestamp { get; private set; }

    public DateTimeOffset? EditedTimestamp { get; private set; }

    public bool IsTTS { get; private set; }

    public bool MentionsEveryone { get; private set; }

    public IReadOnlyList<IUserMention> Mentions { get; private set; } = null!;

    public IReadOnlyList<Snowflake> MentionedRoles { get; private set; } = null!;

    public Optional<IReadOnlyList<IChannelMention>> MentionedChannels { get; private set; }

    public IReadOnlyList<IAttachment> Attachments { get; private set; } = null!;

    public IReadOnlyList<IEmbed> Embeds { get; private set; } = null!;

    public Optional<IReadOnlyList<IReaction>> Reactions { get; private set; }

    public Optional<string> Nonce { get; private set; }

    public bool IsPinned { get; private set; }

    public Optional<Snowflake> WebhookID { get; private set; }

    public MessageType Type { get; private set; }

    public Optional<IMessageActivity> Activity { get; private set; }

    public Optional<IPartialApplication> Application { get; private set; }

    public Optional<Snowflake> ApplicationID { get; private set; }

    public Optional<IMessageReference> MessageReference { get; private set; }

    public Optional<IReadOnlyList<IMessageSnapshot>> MessageSnapshots { get; private set; }

    public Optional<MessageFlags> Flags { get; private set; }

    public Optional<IMessage?> ReferencedMessage { get; private set; }

    public Optional<IMessageInteraction> Interaction { get; private set; }

    public Optional<IChannel> Thread { get; private set; }

    public Optional<IReadOnlyList<IMessageComponent>> Components { get; private set; }

    public Optional<IReadOnlyList<IStickerItem>> StickerItems { get; private set; }

    public Optional<int> Position { get; private set; }

    public Optional<IApplicationCommandInteractionDataResolved> Resolved { get; private set; }

    public Optional<IMessageInteractionMetadata> InteractionMetadata { get; private set; }

    public Optional<IPoll> Poll { get; private set; }

    public Optional<IMessageCall> Call { get; private set; }

    internal void Populate(IMessageCreate m)
    {
        if (m.ID != ID)
            throw new InvalidOperationException("Message ID mismatch");

        if (m.ChannelID != ChannelID)
            throw new InvalidOperationException("Channel ID mismatch");

        GuildID = m.GuildID;
        Author = m.Author;
        Member = m.Member;
        Content = m.Content;
        Timestamp = m.Timestamp;
        EditedTimestamp = m.EditedTimestamp;
        IsTTS = m.IsTTS;
        MentionsEveryone = m.MentionsEveryone;
        Mentions = m.Mentions;
        MentionedRoles = m.MentionedRoles;
        MentionedChannels = m.MentionedChannels;
        Attachments = m.Attachments;
        Embeds = m.Embeds;
        Reactions = m.Reactions;
        Nonce = m.Nonce;
        IsPinned = m.IsPinned;
        WebhookID = m.WebhookID;
        Type = m.Type;
        Activity = m.Activity;
        Application = m.Application;
        ApplicationID = m.ApplicationID;
        MessageReference = m.MessageReference;
        MessageSnapshots = m.MessageSnapshots;
        Flags = m.Flags;
        ReferencedMessage = m.ReferencedMessage;
        Interaction = m.Interaction;
        Thread = m.Thread;
        Components = m.Components;
        StickerItems = m.StickerItems;
        Position = m.Position;
        Resolved = m.Resolved;
        InteractionMetadata = m.InteractionMetadata;
        Poll = m.Poll;
        Call = m.Call;
    }

    internal void Populate(IMessage m)
    {
        if (m.ID != ID)
            throw new InvalidOperationException("Message ID mismatch");

        if (m.ChannelID != ChannelID)
            throw new InvalidOperationException("Channel ID mismatch");

        Author = m.Author;
        Content = m.Content;
        Timestamp = m.Timestamp;
        EditedTimestamp = m.EditedTimestamp;
        IsTTS = m.IsTTS;
        MentionsEveryone = m.MentionsEveryone;
        MentionedRoles = m.MentionedRoles;
        MentionedChannels = m.MentionedChannels;
        Attachments = m.Attachments;
        Embeds = m.Embeds;
        Reactions = m.Reactions;
        Nonce = m.Nonce;
        IsPinned = m.IsPinned;
        WebhookID = m.WebhookID;
        Type = m.Type;
        Activity = m.Activity;
        Application = m.Application;
        ApplicationID = m.ApplicationID;
        MessageReference = m.MessageReference;
        MessageSnapshots = m.MessageSnapshots;
        Flags = m.Flags;
        ReferencedMessage = m.ReferencedMessage;
        Interaction = m.Interaction;
        Thread = m.Thread;
        Components = m.Components;
        StickerItems = m.StickerItems;
        Position = m.Position;
        Resolved = m.Resolved;
        InteractionMetadata = m.InteractionMetadata;
        Poll = m.Poll;
        Call = m.Call;
    }

    internal void Update(IMessageUpdate m)
    {
        if (m.ID.Value != ID)
            throw new InvalidOperationException("Message ID mismatch");

        if (m.ChannelID != ChannelID)
            throw new InvalidOperationException("Channel ID mismatch");

        GuildID = m.GuildID;
        Author = m.Author;
        Member = m.Member;
        Content = m.Content;
        Timestamp = m.Timestamp;
        EditedTimestamp = m.EditedTimestamp;
        IsTTS = m.IsTTS;
        MentionsEveryone = m.MentionsEveryone;
        Mentions = m.Mentions;
        MentionedRoles = m.MentionedRoles;
        MentionedChannels = m.MentionedChannels;
        Attachments = m.Attachments;
        Embeds = m.Embeds;
        Reactions = m.Reactions;
        Nonce = m.Nonce;
        IsPinned = m.IsPinned;
        WebhookID = m.WebhookID;
        Type = m.Type;
        Activity = m.Activity;
        Application = m.Application;
        ApplicationID = m.ApplicationID;
        MessageReference = m.MessageReference;
        MessageSnapshots = m.MessageSnapshots;
        Flags = m.Flags;
        ReferencedMessage = m.ReferencedMessage;
        Interaction = m.Interaction;
        Thread = m.Thread;
        Components = m.Components;
        StickerItems = m.StickerItems;
        Position = m.Position;
        Resolved = m.Resolved;
        InteractionMetadata = m.InteractionMetadata;
        Poll = m.Poll;
        Call = m.Call;
    }
}
