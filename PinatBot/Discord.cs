using System.Text.Json;
using Microsoft.Extensions.Options;
using PinatBot.Caching;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway;

namespace PinatBot;

public class Discord
{
    public Discord(DiscordGatewayClient gatewayClient,
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
        GatewayClient = gatewayClient;
        JsonSerializerOptions = new JsonSerializerOptions(jsonOptions.Get("Discord")) { WriteIndented = true };
        Rest = new DiscordRest(application, auditLog, channel, emoji, gateway, guild, guildScheduledEvent, interaction, invite, oAuth2, stageInstance, sticker, template, user, voice, webhook);
        GatewayCache = gatewayCache;
    }

    public DiscordGatewayClient GatewayClient { get; }
    public JsonSerializerOptions JsonSerializerOptions { get; }
    public DiscordRest Rest { get; }
    public DiscordGatewayCache GatewayCache { get; }

    public class DiscordRest
    {
        public DiscordRest(IDiscordRestApplicationAPI application,
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
            Application = application;
            AuditLog = auditLog;
            Channel = channel;
            Emoji = emoji;
            Gateway = gateway;
            Guild = guild;
            GuildScheduledEvent = guildScheduledEvent;
            Interaction = interaction;
            Invite = invite;
            OAuth2 = oAuth2;
            StageInstance = stageInstance;
            Sticker = sticker;
            Template = template;
            User = user;
            Voice = voice;
            Webhook = webhook;
        }

        public IDiscordRestApplicationAPI Application { get; }
        public IDiscordRestAuditLogAPI AuditLog { get; }
        public IDiscordRestChannelAPI Channel { get; }
        public IDiscordRestEmojiAPI Emoji { get; }
        public IDiscordRestGatewayAPI Gateway { get; }
        public IDiscordRestGuildAPI Guild { get; }
        public IDiscordRestGuildScheduledEventAPI GuildScheduledEvent { get; }
        public IDiscordRestInteractionAPI Interaction { get; }
        public IDiscordRestInviteAPI Invite { get; }
        public IDiscordRestOAuth2API OAuth2 { get; }
        public IDiscordRestStageInstanceAPI StageInstance { get; }
        public IDiscordRestStickerAPI Sticker { get; }
        public IDiscordRestTemplateAPI Template { get; }
        public IDiscordRestUserAPI User { get; }
        public IDiscordRestVoiceAPI Voice { get; }
        public IDiscordRestWebhookAPI Webhook { get; }
    }
}
