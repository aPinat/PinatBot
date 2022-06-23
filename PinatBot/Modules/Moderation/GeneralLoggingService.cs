using System.Drawing;
using Microsoft.EntityFrameworkCore;
using OneOf;
using PinatBot.Data;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Extensions.Embeds;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class GeneralLoggingService
{
    private readonly IDbContextFactory<Database> _dbContextFactory;
    private readonly Discord _discord;
    private readonly HttpClient _httpClient = new();

    public GeneralLoggingService(IDbContextFactory<Database> dbContextFactory, Discord discord)
    {
        _dbContextFactory = dbContextFactory;
        _discord = discord;
    }

    private async Task<Result> SendLogMessageAsync(ulong channelId, EmbedBuilder builder, List<OneOf<FileData, IPartialAttachment>>? attachments = null, CancellationToken cancellationToken = default)
    {
        var buildResult = builder.Build();
        if (!buildResult.IsDefined(out var embed))
            return Result.FromError(buildResult);

        var id = new Snowflake(channelId);
        var messageResult = await _discord.Rest.Channel.CreateMessageAsync(id, embeds: new[] { embed }, ct: cancellationToken);
        if (attachments is null)
            return messageResult.IsSuccess ? Result.FromSuccess() : Result.FromError(messageResult);

        if (!messageResult.IsDefined(out var message))
            return Result.FromError(messageResult);
        var attachmentResult =
            await _discord.Rest.Channel.CreateMessageAsync(id, "Deleted Attachment(s):", attachments: attachments, messageReference: new MessageReference(message.ID), ct: cancellationToken);
        return messageResult.IsSuccess && attachmentResult.IsSuccess ? Result.FromSuccess() :
            messageResult.IsSuccess ? Result.FromError(attachmentResult) : Result.FromError(messageResult);
    }

    public async Task<Result> LogMessageUpdatedAsync(IMessageUpdate m, CancellationToken cancellationToken = default)
    {
        if (!m.GuildID.IsDefined(out var guildId) || !m.ChannelID.IsDefined(out var channelId) || !m.ID.IsDefined(out var messageId) || !m.Author.IsDefined(out var author) ||
            (author.IsBot.IsDefined(out var isBot) && isBot) || m.WebhookID.HasValue || m.ApplicationID.HasValue || !m.EditedTimestamp.HasValue)
            return Result.FromSuccess();

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.GeneralLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == guildId.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var cacheResult = await _discord.GatewayCache.GetMessageAsync(messageId, channelId, cancellationToken);
        var beforeContent = !cacheResult.IsDefined(out var previousMessage) ? "_Message not in cache!_" :
            string.IsNullOrEmpty(previousMessage.Content) ? "_Message has no content!_" : previousMessage.Content;
        var afterContent = m.Content.IsDefined(out var content) && !string.IsNullOrEmpty(content) ? content : "_Message has no content!_";

        var thumbnailUrl = author.AvatarUrl().ToString();
        var builder = new EmbedBuilder
        {
            Title = "Message Edited",
            Colour = Color.Orange,
            Timestamp = DateTimeOffset.Now,
            ThumbnailUrl = thumbnailUrl,
            Author = new EmbedAuthorBuilder($"{author.DiscordTag()}", iconUrl: thumbnailUrl)
        };

        if (beforeContent.Length < 1024 && afterContent.Length < 1024)
        {
            builder.AddField("Original Message", beforeContent);
            builder.AddField("Edited Message", afterContent);
        }
        else if (beforeContent.Length + afterContent.Length < 4000)
        {
            builder.Description = $"**Original Message**\n{beforeContent}\n\n**Edited Message**\n{afterContent}";
        }
        else
        {
            builder.Description = $"**Original Message**\n{beforeContent}\n\n**Edited Message**\n_Check link below!_";
        }

        builder.AddField("Author", author.Mention(), true);
        builder.AddField("Channel", $"<#{channelId}>", true);
        builder.AddField("Message ID", $"[{messageId}]({m.Link(guildId, channelId, messageId)})");

        if (previousMessage is null)
            goto SEND;

        var embedBefore = previousMessage.Embeds.Count;
        var embedAfter = m.Embeds.IsDefined(out var embeds) ? embeds.Count.ToString() : "None";
        builder.AddField("Embeds", $"{embedBefore} >> {embedAfter}", true);

        var attachmentBefore = previousMessage.Attachments.Count;
        var attachmentAfter = m.Attachments.IsDefined(out var attachments) ? attachments.Count.ToString() : "None";
        builder.AddField("Attachments", $"{attachmentBefore} >> {attachmentAfter}", true);

        if (!previousMessage.Attachments.Any() || attachmentBefore == attachments?.Count)
            goto SEND;

        var attachmentsLog = new List<OneOf<FileData, IPartialAttachment>>();
        var removedAttachments = attachments is null ? previousMessage.Attachments : previousMessage.Attachments.Except(attachments);
        foreach (var attachment in removedAttachments)
        {
            var stream = await _httpClient.GetStreamAsync(attachment.Url, cancellationToken);
            var guid = Guid.NewGuid();
            var newFilename = $"{guid}.{attachment.Filename}";
            attachmentsLog.Add(new FileData(newFilename, stream));
        }

        return await SendLogMessageAsync(logging.ChannelId, builder, attachmentsLog, cancellationToken);

    SEND:
        return await SendLogMessageAsync(logging.ChannelId, builder, cancellationToken: cancellationToken);
    }

    public async Task<Result> LogMessageDeletedAsync(IMessageDelete m, CancellationToken cancellationToken = default)
    {
        if (!m.GuildID.IsDefined(out var guildId))
            return Result.FromSuccess();

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.GeneralLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == guildId.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var builder = new EmbedBuilder { Title = "Message Deleted", Colour = Color.Red, Timestamp = DateTimeOffset.Now };

        var channelId = m.ChannelID;
        var messageId = m.ID;

        var cacheResult = await _discord.GatewayCache.GetMessageAsync(messageId, channelId, cancellationToken);
        if (!cacheResult.IsDefined(out var cachedMessage))
        {
            builder.AddField("Channel", $"<#{channelId}>", true);
            builder.AddField("Message ID", messageId.ToString());
            goto SEND;
        }

        if ((cachedMessage.Author.IsBot.IsDefined(out var isBot) && isBot) || cachedMessage.WebhookID.HasValue || cachedMessage.ApplicationID.HasValue)
            return Result.FromSuccess();

        var beforeContent = string.IsNullOrEmpty(cachedMessage.Content) ? "_Message has no content!_" : cachedMessage.Content;
        var thumbnailUrl = cachedMessage.Author.AvatarUrl().ToString();

        builder.ThumbnailUrl = thumbnailUrl;
        builder.Author = new EmbedAuthorBuilder($"{cachedMessage.Author.DiscordTag()}", iconUrl: thumbnailUrl);
        if (beforeContent.Length < 1024)
            builder.AddField("Original Message", beforeContent);
        else
            builder.Description = $"**Original Message**\n{beforeContent}";
        builder.AddField("Author", cachedMessage.Author.Mention(), true);
        builder.AddField("Channel", $"<#{channelId}>", true);
        builder.AddField("Message ID", $"[{messageId}]({cachedMessage.Link(guildId, channelId, messageId)})");
        builder.AddField("Embeds", cachedMessage.Embeds.Count.ToString(), true);
        builder.AddField("Attachments", cachedMessage.Attachments.Count.ToString(), true);


        if (!cachedMessage.Attachments.Any())
            goto SEND;

        var attachments = new List<OneOf<FileData, IPartialAttachment>>();
        foreach (var attachment in cachedMessage.Attachments)
        {
            var stream = await _httpClient.GetStreamAsync(attachment.Url, cancellationToken);
            var guid = Guid.NewGuid();
            var newFilename = $"{guid}.{attachment.Filename}";
            attachments.Add(new FileData(newFilename, stream));
        }

        return await SendLogMessageAsync(logging.ChannelId, builder, attachments, cancellationToken);

    SEND:
        return await SendLogMessageAsync(logging.ChannelId, builder, cancellationToken: cancellationToken);
    }

    public async Task<Result> LogGuildMemberAddedAsync(IGuildMemberAdd m, CancellationToken cancellationToken = default)
    {
        if (!m.User.IsDefined(out var user))
            return Result.FromError(new ArgumentInvalidError(nameof(m.User), "User is not defined"));

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.GeneralLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == m.GuildID.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var builder = new EmbedBuilder { Title = "Member Joined", Colour = Color.Lime, Timestamp = DateTimeOffset.Now, ThumbnailUrl = user.AvatarUrl().ToString() };
        builder.AddField("Member", $"{user.Mention()} {user.DiscordTag()}");
        builder.AddField("Member ID", user.ID.ToString(), true);
        builder.AddField("Created", user.ID.Timestamp.ToDiscordTimestamp(), true);

        return await SendLogMessageAsync(logging.ChannelId, builder, cancellationToken: cancellationToken);
    }

    public async Task<Result> LogGuildMemberRemovedAsync(IGuildMemberRemove m, CancellationToken cancellationToken = default)
    {
        var time = DateTimeOffset.UtcNow;

        await using var database = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var logging = database.GeneralLoggingConfigs.AsNoTracking().FirstOrDefault(config => config.GuildId == m.GuildID.Value);
        if (logging is not { Enabled: true })
            return Result.FromSuccess();

        var builder = new EmbedBuilder { Title = "Member Left", Colour = Color.Red, Timestamp = DateTimeOffset.Now, ThumbnailUrl = m.User.AvatarUrl().ToString() };
        builder.AddField("Member", $"{m.User.Mention()} {m.User.DiscordTag()}");
        builder.AddField("Member ID", m.User.ID.ToString(), true);
        builder.AddField("Created", m.User.ID.Timestamp.ToDiscordTimestamp(), true);

        var auditLogResult = await _discord.Rest.AuditLog.GetAuditLogAsync(m.GuildID, actionType: AuditLogEvent.MemberKick, limit: 5, ct: cancellationToken);
        if (auditLogResult.IsDefined(out var auditLog))
        {
            var entry = auditLog.AuditLogEntries.FirstOrDefault(e => e.TargetID == m.User.ID.ToString() && e.ID.Timestamp.Subtract(time).TotalSeconds is > -10 and < 10);
            if (entry is not null)
            {
                builder.Title = "Member Kicked";
                var user = auditLog.Users.FirstOrDefault(user => user.ID == entry.UserID);
                builder.AddField("Kicked by", user is null ? $"<@{entry.UserID.ToString()}>" : $"{user.Mention()} {user.DiscordTag()}");
                if (entry.Reason.IsDefined(out var reason) && !string.IsNullOrEmpty(reason))
                    builder.AddField("Reason", reason, true);
            }
        }

        auditLogResult = await _discord.Rest.AuditLog.GetAuditLogAsync(m.GuildID, actionType: AuditLogEvent.MemberBanAdd, limit: 5, ct: cancellationToken);
        if (!auditLogResult.IsDefined(out auditLog))
            goto SEND;

        {
            var entry = auditLog.AuditLogEntries.FirstOrDefault(e => e.TargetID == m.User.ID.ToString() && e.ID.Timestamp.Subtract(time).TotalSeconds is > -10 and < 10);
            if (entry is null)
                goto SEND;

            builder.Title = "Member Banned";
            var user = auditLog.Users.FirstOrDefault(user => user.ID == entry.UserID);
            builder.AddField("Banned by", user is null ? $"<@{entry.UserID.ToString()}>" : $"{user.Mention()} {user.DiscordTag()}");
            if (entry.Reason.IsDefined(out var reason) && !string.IsNullOrEmpty(reason))
                builder.AddField("Reason", reason, true);
        }

    SEND:
        return await SendLogMessageAsync(logging.ChannelId, builder, cancellationToken: cancellationToken);
    }
}
