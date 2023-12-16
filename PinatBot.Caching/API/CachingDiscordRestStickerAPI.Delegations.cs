using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestStickerAPI
{
    public Task<Result<ISticker>> GetStickerAsync(Snowflake stickerID, CancellationToken ct = default) => actual.GetStickerAsync(stickerID, ct);

    public Task<Result<INitroStickerPacks>> ListNitroStickerPacksAsync(CancellationToken ct = default) => actual.ListNitroStickerPacksAsync(ct);

    public Task<Result<ISticker>> CreateGuildStickerAsync(Snowflake guildID,
        string name,
        string description,
        string tags,
        FileData file,
        Optional<string> reason = default,
        CancellationToken ct = default) => actual.CreateGuildStickerAsync(guildID, name, description, tags, file, reason, ct);

    public Task<Result<ISticker>> ModifyGuildStickerAsync(Snowflake guildID,
        Snowflake stickerID,
        Optional<string> name = default,
        Optional<string?> description = default,
        Optional<string> tags = default,
        Optional<string> reason = default,
        CancellationToken ct = default) =>
        actual.ModifyGuildStickerAsync(guildID, stickerID, name, description, tags, reason, ct);

    public Task<Result> DeleteGuildStickerAsync(Snowflake guildID, Snowflake stickerID, Optional<string> reason = default, CancellationToken ct = default) =>
        actual.DeleteGuildStickerAsync(guildID, stickerID, reason, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (actual as IRestCustomizable)?.RemoveCustomization(customization);
}
