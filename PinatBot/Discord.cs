using System.Text.Json;
using Microsoft.Extensions.Options;
using PinatBot.Caching;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway;

namespace PinatBot;

public class Discord(
    DiscordGatewayClient gatewayClient,
    IOptionsMonitor<JsonSerializerOptions> jsonOptions,
    IDiscordRestApplicationAPI application,
    IDiscordRestAuditLogAPI auditLog,
    IDiscordRestChannelAPI channel,
    IDiscordRestEmojiAPI emoji,
    IDiscordRestGatewayAPI gateway,
    IDiscordRestGuildAPI guild,
    IDiscordRestGuildScheduledEventAPI guildScheduledEvent,
    IDiscordRestInteractionAPI interaction,
    IDiscordRestInviteAPI invite,
    IDiscordRestOAuth2API oAuth2,
    IDiscordRestStageInstanceAPI stageInstance,
    IDiscordRestStickerAPI sticker,
    IDiscordRestTemplateAPI template,
    IDiscordRestUserAPI user,
    IDiscordRestVoiceAPI voice,
    IDiscordRestWebhookAPI webhook,
    DiscordGatewayCache gatewayCache)
{
    public DiscordGatewayClient GatewayClient { get; } = gatewayClient;
    public JsonSerializerOptions JsonSerializerOptions { get; } = new(jsonOptions.Get("Discord")) { WriteIndented = true };

    public DiscordRest Rest { get; } = new(application, auditLog, channel, emoji, gateway, guild, guildScheduledEvent, interaction, invite, oAuth2, stageInstance, sticker, template, user, voice,
        webhook);

    public DiscordGatewayCache GatewayCache { get; } = gatewayCache;

    public class DiscordRest(
        IDiscordRestApplicationAPI application,
        IDiscordRestAuditLogAPI auditLog,
        IDiscordRestChannelAPI channel,
        IDiscordRestEmojiAPI emoji,
        IDiscordRestGatewayAPI gateway,
        IDiscordRestGuildAPI guild,
        IDiscordRestGuildScheduledEventAPI guildScheduledEvent,
        IDiscordRestInteractionAPI interaction,
        IDiscordRestInviteAPI invite,
        IDiscordRestOAuth2API oAuth2,
        IDiscordRestStageInstanceAPI stageInstance,
        IDiscordRestStickerAPI sticker,
        IDiscordRestTemplateAPI template,
        IDiscordRestUserAPI user,
        IDiscordRestVoiceAPI voice,
        IDiscordRestWebhookAPI webhook)
    {
        public IDiscordRestApplicationAPI Application { get; } = application;
        public IDiscordRestAuditLogAPI AuditLog { get; } = auditLog;
        public IDiscordRestChannelAPI Channel { get; } = channel;
        public IDiscordRestEmojiAPI Emoji { get; } = emoji;
        public IDiscordRestGatewayAPI Gateway { get; } = gateway;
        public IDiscordRestGuildAPI Guild { get; } = guild;
        public IDiscordRestGuildScheduledEventAPI GuildScheduledEvent { get; } = guildScheduledEvent;
        public IDiscordRestInteractionAPI Interaction { get; } = interaction;
        public IDiscordRestInviteAPI Invite { get; } = invite;
        public IDiscordRestOAuth2API OAuth2 { get; } = oAuth2;
        public IDiscordRestStageInstanceAPI StageInstance { get; } = stageInstance;
        public IDiscordRestStickerAPI Sticker { get; } = sticker;
        public IDiscordRestTemplateAPI Template { get; } = template;
        public IDiscordRestUserAPI User { get; } = user;
        public IDiscordRestVoiceAPI Voice { get; } = voice;
        public IDiscordRestWebhookAPI Webhook { get; } = webhook;
    }
}
