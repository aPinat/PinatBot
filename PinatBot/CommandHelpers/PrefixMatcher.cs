using PinatBot.Caching;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PrefixMatcher : ICommandPrefixMatcher
{
    private readonly DiscordGatewayCache _cache;
    private readonly Configuration _configuration;
    private readonly MessageContext _messageContext;

    public PrefixMatcher(DiscordGatewayCache cache, Configuration configuration, MessageContext messageContext)
    {
        _cache = cache;
        _configuration = configuration;
        _messageContext = messageContext;
    }

    public ValueTask<Result<(bool Matches, int ContentStartIndex)>> MatchesPrefixAsync(string content, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
            goto FALSE;

        var prefix = _configuration.Discord.Prefix;
        if (content.StartsWith(prefix, StringComparison.Ordinal))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, prefix.Length)));

        var meResult = _cache.GetCurrentUser();
        if (!meResult.IsDefined(out var user))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromError(meResult));

        var mention = user.Mention();
        if (content.StartsWith(mention, StringComparison.Ordinal))
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, mention.Length)));

        if (!_messageContext.GuildID.HasValue)
            return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, 0)));

    FALSE:
        return ValueTask.FromResult(Result<(bool Matches, int ContentStartIndex)>.FromSuccess((false, 0)));
    }
}
