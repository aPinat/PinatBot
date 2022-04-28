using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace PinatBot.CommandHelpers;

public class PrefixMatcher : ICommandPrefixMatcher
{
    private readonly Configuration _configuration;
    private readonly Discord _discord;
    private readonly MessageContext _messageContext;

    public PrefixMatcher(Configuration configuration, MessageContext messageContext, Discord discord)
    {
        _discord = discord;
        _configuration = configuration;
        _messageContext = messageContext;
    }

    public async ValueTask<Result<(bool Matches, int ContentStartIndex)>> MatchesPrefixAsync(string content, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
            goto FALSE;

        var prefix = _configuration.Discord.Prefix;
        if (content.StartsWith(prefix, StringComparison.Ordinal))
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, prefix.Length));

        var meResult = await _discord.Rest.User.GetCurrentUserAsync(ct);
        if (!meResult.IsDefined(out var user))
            return Result<(bool Matches, int ContentStartIndex)>.FromError(meResult);

        var mention = user.Mention();
        if (content.StartsWith(mention, StringComparison.Ordinal))
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, mention.Length));

        if (!_messageContext.GuildID.HasValue)
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, 0));

        FALSE:
        return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((false, 0));
    }
}
