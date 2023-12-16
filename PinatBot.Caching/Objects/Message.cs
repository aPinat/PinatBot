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

    public Optional<MessageFlags> Flags { get; private set; }

    public Optional<IMessage?> ReferencedMessage { get; private set; }

    public Optional<IMessageInteraction> Interaction { get; private set; }

    public Optional<IChannel> Thread { get; private set; }

    public Optional<IReadOnlyList<IMessageComponent>> Components { get; private set; }

    public Optional<IReadOnlyList<IStickerItem>> StickerItems { get; private set; }

    public Optional<int> Position { get; private set; }

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
        Flags = m.Flags;
        ReferencedMessage = m.ReferencedMessage;
        Interaction = m.Interaction;
        Thread = m.Thread;
        Components = m.Components;
        StickerItems = m.StickerItems;
        Position = m.Position;
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
        Flags = m.Flags;
        ReferencedMessage = m.ReferencedMessage;
        Interaction = m.Interaction;
        Thread = m.Thread;
        Components = m.Components;
        StickerItems = m.StickerItems;
        Position = m.Position;
    }

    internal void Update(IMessageUpdate m)
    {
        if (m.ID.Value != ID)
            throw new InvalidOperationException("Message ID mismatch");

        if (m.ChannelID.IsDefined(out var channelID) && channelID != ChannelID)
            throw new InvalidOperationException("Channel ID mismatch");

        if (m.GuildID.IsDefined(out var guildID))
            GuildID = guildID;

        if (m.Author.IsDefined(out var author))
            Author = author;

        if (m.Member.IsDefined(out var member))
            Member = new Optional<IPartialGuildMember>(member);

        if (m.Content.IsDefined(out var content))
            Content = content;

        if (m.Timestamp.IsDefined(out var timestamp))
            Timestamp = timestamp;

        if (m.EditedTimestamp.IsDefined(out var editedTimestamp))
            EditedTimestamp = editedTimestamp;

        if (m.IsTTS.IsDefined(out var isTTS))
            IsTTS = isTTS;

        if (m.MentionsEveryone.IsDefined(out var mentionsEveryone))
            MentionsEveryone = mentionsEveryone;

        if (m.Mentions.IsDefined(out var mentions))
            Mentions = mentions;

        if (m.MentionedRoles.IsDefined(out var mentionedRoles))
            MentionedRoles = mentionedRoles;

        if (m.MentionedChannels.IsDefined(out var mentionedChannels))
            MentionedChannels = new Optional<IReadOnlyList<IChannelMention>>(mentionedChannels);

        if (m.Attachments.IsDefined(out var attachments))
            Attachments = attachments;

        if (m.Embeds.IsDefined(out var embeds))
            Embeds = embeds;

        if (m.Reactions.IsDefined(out var reactions))
            Reactions = new Optional<IReadOnlyList<IReaction>>(reactions);

        if (m.Nonce.IsDefined(out var nonce))
            Nonce = nonce;

        if (m.IsPinned.IsDefined(out var isPinned))
            IsPinned = isPinned;

        if (m.WebhookID.IsDefined(out var webhookID))
            WebhookID = webhookID;

        if (m.Type.IsDefined(out var type))
            Type = type;

        if (m.Activity.IsDefined(out var activity))
            Activity = new Optional<IMessageActivity>(activity);

        if (m.Application.IsDefined(out var application))
            Application = new Optional<IPartialApplication>(application);

        if (m.ApplicationID.IsDefined(out var applicationID))
            ApplicationID = applicationID;

        if (m.MessageReference.IsDefined(out var messageReference))
            MessageReference = new Optional<IMessageReference>(messageReference);

        if (m.Flags.IsDefined(out var flags))
            Flags = flags;

        if (m.ReferencedMessage.IsDefined(out var referencedMessage))
            ReferencedMessage = new Optional<IMessage?>(referencedMessage);

        if (m.Interaction.IsDefined(out var interaction))
            Interaction = new Optional<IMessageInteraction>(interaction);

        if (m.Thread.IsDefined(out var thread))
            Thread = new Optional<IChannel>(thread);

        if (m.Components.IsDefined(out var components))
            Components = new Optional<IReadOnlyList<IMessageComponent>>(components);

        if (m.StickerItems.IsDefined(out var stickerItems))
            StickerItems = new Optional<IReadOnlyList<IStickerItem>>(stickerItems);

        if (m.Position.IsDefined(out var position))
            Position = position;
    }
}
