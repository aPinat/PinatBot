using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using PinatBot.Caching.API;
using PinatBot.Caching.Responders;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Responders;
using Remora.Discord.Rest.Extensions;

namespace PinatBot.Caching;

public static class Extensions
{
    public static IServiceCollection AddPinatBotCaching(this IServiceCollection services, Action<RedisCacheOptions> redisCacheOptionsAction) =>
        services
            .AddSingleton<DiscordGatewayCache>()
            .Decorate<IDiscordRestChannelAPI, CachingDiscordRestChannelAPI>()
            .Decorate<IDiscordRestEmojiAPI, CachingDiscordRestEmojiAPI>()
            .Decorate<IDiscordRestGuildAPI, CachingDiscordRestGuildAPI>()
            .Decorate<IDiscordRestStickerAPI, CachingDiscordRestStickerAPI>()
            .Decorate<IDiscordRestUserAPI, CachingDiscordRestUserAPI>()
            .AddStackExchangeRedisCache(redisCacheOptionsAction)
            .AddSingleton<DistributedCacheProvider>()
            .AddResponder<ChannelCreate>(ResponderGroup.Early)
            .AddResponder<ChannelDelete>(ResponderGroup.Late)
            .AddResponder<ChannelUpdate>(ResponderGroup.Late)
            .AddResponder<GuildCreate>(ResponderGroup.Early)
            .AddResponder<GuildDelete>(ResponderGroup.Late)
            .AddResponder<GuildEmojisUpdate>(ResponderGroup.Late)
            .AddResponder<GuildMemberAdd>(ResponderGroup.Early)
            .AddResponder<GuildMemberChunk>(ResponderGroup.Early)
            .AddResponder<GuildMemberRemove>(ResponderGroup.Late)
            .AddResponder<GuildMemberUpdate>(ResponderGroup.Late)
            .AddResponder<GuildRoleCreate>(ResponderGroup.Early)
            .AddResponder<GuildRoleDelete>(ResponderGroup.Late)
            .AddResponder<GuildRoleUpdate>(ResponderGroup.Late)
            .AddResponder<GuildScheduledEventCreate>(ResponderGroup.Early)
            .AddResponder<GuildScheduledEventDelete>(ResponderGroup.Late)
            .AddResponder<GuildScheduledEventUpdate>(ResponderGroup.Late)
            .AddResponder<GuildStickersUpdate>(ResponderGroup.Late)
            .AddResponder<GuildUpdate>(ResponderGroup.Late)
            .AddResponder<MessageCreate>(ResponderGroup.Early)
            .AddResponder<MessageDelete>(ResponderGroup.Late)
            .AddResponder<MessageUpdate>(ResponderGroup.Late)
            .AddResponder<PresenceUpdate>(ResponderGroup.Late)
            .AddResponder<Ready>(ResponderGroup.Early)
            .AddResponder<StageInstanceCreate>(ResponderGroup.Early)
            .AddResponder<StageInstanceDelete>(ResponderGroup.Late)
            .AddResponder<StageInstanceUpdate>(ResponderGroup.Late)
            .AddResponder<ThreadCreate>(ResponderGroup.Early)
            .AddResponder<ThreadListSync>(ResponderGroup.Early)
            .AddResponder<ThreadUpdate>(ResponderGroup.Late)
            .AddResponder<UserUpdate>(ResponderGroup.Late)
            .AddResponder<VoiceStateUpdate>(ResponderGroup.Late);
}
