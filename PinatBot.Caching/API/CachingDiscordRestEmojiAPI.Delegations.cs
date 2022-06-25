using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestEmojiAPI
{
    public Task<Result<IEmoji>>
        CreateGuildEmojiAsync(Snowflake guildID, string name, Stream image, IReadOnlyList<Snowflake> roles, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.CreateGuildEmojiAsync(guildID, name, image, roles, reason, ct);

    public Task<Result<IEmoji>> ModifyGuildEmojiAsync(Snowflake guildID,
        Snowflake emojiID,
        Optional<string> name = default,
        Optional<IReadOnlyList<Snowflake>?> roles = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        _actual.ModifyGuildEmojiAsync(guildID, emojiID, name, roles, reason, ct);

    public Task<Result> DeleteGuildEmojiAsync(Snowflake guildID, Snowflake emojiID, Optional<string> reason = default, CancellationToken ct = default) =>
        _actual.DeleteGuildEmojiAsync(guildID, emojiID, reason, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (_actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (_actual as IRestCustomizable)?.RemoveCustomization(customization);
}
