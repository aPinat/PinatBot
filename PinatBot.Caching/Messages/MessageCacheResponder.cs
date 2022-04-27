using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Caching;
using Remora.Discord.Caching.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Messages;

public class MessageCacheResponder : IResponder<IMessageUpdate>
{
    public MessageCacheResponder(CacheService cache) => Cache = cache;
    private CacheService Cache { get; }

    public async Task<Result> RespondAsync(IMessageUpdate m, CancellationToken ct = default)
    {
        if (!m.ChannelID.IsDefined(out var channelId) || !m.ID.IsDefined(out var messageId) || !m.Author.IsDefined(out var author) ||
            (author.IsBot.IsDefined(out var isBot) && isBot) || m.WebhookID.HasValue || m.ApplicationID.HasValue || !m.EditedTimestamp.HasValue)
            return Result.FromSuccess();

        var key = KeyHelpers.CreateMessageCacheKey(channelId, messageId);

        var cachedResult = await Cache.TryGetValueAsync<IMessage>(key, ct);
        if (!cachedResult.IsDefined(out var cachedMessage))
            return Result.FromError(cachedResult);

        await Cache.CacheAsync(key,
            new Message(messageId,
                channelId,
                cachedMessage.GuildID,
                cachedMessage.Author,
                cachedMessage.Member,
                m.Content.IsDefined(out var content) ? content : cachedMessage.Content,
                cachedMessage.Timestamp,
                m.EditedTimestamp.HasValue ? m.EditedTimestamp.Value : cachedMessage.EditedTimestamp,
                cachedMessage.IsTTS,
                cachedMessage.MentionsEveryone,
                cachedMessage.Mentions,
                cachedMessage.MentionedRoles,
                cachedMessage.MentionedChannels,
                m.Attachments.IsDefined(out var attachments) ? attachments : cachedMessage.Attachments,
                m.Embeds.IsDefined(out var embeds) ? embeds : cachedMessage.Embeds,
                m.Reactions.HasValue ? m.Reactions : cachedMessage.Reactions,
                cachedMessage.Nonce,
                m.IsPinned.IsDefined(out var isPinned) ? isPinned : cachedMessage.IsPinned,
                cachedMessage.WebhookID,
                cachedMessage.Type,
                cachedMessage.Activity,
                cachedMessage.Application,
                cachedMessage.ApplicationID,
                cachedMessage.MessageReference,
                m.Flags.HasValue ? m.Flags : cachedMessage.Flags,
                cachedMessage.ReferencedMessage,
                cachedMessage.Interaction,
                cachedMessage.Thread,
                m.Components.HasValue ? m.Components : cachedMessage.Components,
                cachedMessage.StickerItems), ct);

        return Result.FromSuccess();
    }
}
