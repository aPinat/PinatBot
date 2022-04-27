using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using PinatBot.Caching.Messages;
using PinatBot.Caching.Presences;
using PinatBot.Caching.VoiceStates;
using Remora.Discord.Caching.Abstractions.Services;
using Remora.Discord.Caching.Extensions;
using Remora.Discord.Caching.Services;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Gateway.Responders;

namespace PinatBot.Caching;

public static class Extensions
{
    public static IServiceCollection AddDiscordMixedCaching(this IServiceCollection services, Action<RedisCacheOptions>? redisCacheOptionsAction) =>
        services
            .AddDiscordCaching()
            .AddStackExchangeRedisCache(redisCacheOptionsAction)
            .AddSingleton<MixedCacheProvider>()
            .AddSingleton<ICacheProvider>(provider => provider.GetRequiredService<MixedCacheProvider>())
            .Configure<CacheSettings>(settings => settings
                .SetDefaultAbsoluteExpiration(null)
                .SetDefaultSlidingExpiration(null))
            .AddSingleton<PresenceCacheService>()
            .AddResponder<PresenceCacheResponder>(ResponderGroup.Late)
            .AddSingleton<VoiceStateCacheService>()
            .AddResponder<VoiceStateCacheResponder>(ResponderGroup.Late)
            .AddResponder<MessageCacheResponder>(ResponderGroup.Late);
}
