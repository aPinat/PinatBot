using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestUserAPI
{
    public Task<Result<IUser>> ModifyCurrentUserAsync(Optional<string> username, Optional<Stream?> avatar = default, CancellationToken ct = default) =>
        _actual.ModifyCurrentUserAsync(username, avatar, ct);

    public Task<Result<IReadOnlyList<IPartialGuild>>> GetCurrentUserGuildsAsync(Optional<Snowflake> before = default,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        CancellationToken ct = default) => _actual.GetCurrentUserGuildsAsync(before, after, limit, ct);

    public Task<Result> LeaveGuildAsync(Snowflake guildID, CancellationToken ct = default) => _actual.LeaveGuildAsync(guildID, ct);

    public Task<Result<IReadOnlyList<IChannel>>> GetUserDMsAsync(CancellationToken ct = default) => _actual.GetUserDMsAsync(ct);

    public Task<Result<IChannel>> CreateDMAsync(Snowflake recipientID, CancellationToken ct = default) => _actual.CreateDMAsync(recipientID, ct);

    public Task<Result<IReadOnlyList<IConnection>>> GetUserConnectionsAsync(CancellationToken ct = default) => _actual.GetUserConnectionsAsync(ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (_actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (_actual as IRestCustomizable)?.RemoveCustomization(customization);
}
