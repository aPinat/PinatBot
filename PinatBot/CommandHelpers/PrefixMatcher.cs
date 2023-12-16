using PinatBot.Caching;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PrefixMatcher(DiscordGatewayCache cache, Configuration configuration, IMessageContext messageContext) : ICommandPrefixMatcher
{
    public ValueTask<Result<(bool Matches, int ContentStartIndex)>> MatchesPrefixAsync(string content, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
            goto FALSE;

        var prefix = configuration.Discord.Prefix;
        if (content.StartsWith(prefix, StringComparison.Ordinal))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, prefix.Length)));

        var meResult = cache.GetCurrentUser();
        if (!meResult.IsDefined(out var user))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromError(meResult));

        var mention = user.Mention();
        if (content.StartsWith(mention, StringComparison.Ordinal))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, mention.Length)));

        if (!messageContext.GuildID.HasValue)
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, 0)));

    FALSE:
        return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((false, 0)));
    }
}
