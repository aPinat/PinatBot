using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.API;

public partial class CachingDiscordRestUserAPI
{
    public Task<Result<IUser>> ModifyCurrentUserAsync(Optional<string> username, Optional<Stream?> avatar = default, CancellationToken ct = default) =>
        actual.ModifyCurrentUserAsync(username, avatar, ct);

    public Task<Result<IReadOnlyList<IPartialGuild>>> GetCurrentUserGuildsAsync(Optional<Snowflake> before = default,
        Optional<Snowflake> after = default,
        Optional<int> limit = default,
        Optional<bool> withCounts = default,
        CancellationToken ct = default) => actual.GetCurrentUserGuildsAsync(before, after, limit, withCounts, ct);

    public Task<Result> LeaveGuildAsync(Snowflake guildID, CancellationToken ct = default) => actual.LeaveGuildAsync(guildID, ct);

    public Task<Result<IReadOnlyList<IChannel>>> GetUserDMsAsync(CancellationToken ct = default) => actual.GetUserDMsAsync(ct);

    public Task<Result<IChannel>> CreateDMAsync(Snowflake recipientID, CancellationToken ct = default) => actual.CreateDMAsync(recipientID, ct);

    public Task<Result<IReadOnlyList<IConnection>>> GetUserConnectionsAsync(CancellationToken ct = default) => actual.GetUserConnectionsAsync(ct);

    public Task<Result<IApplicationRoleConnection>> GetUserApplicationRoleConnectionAsync(Snowflake applicationID, CancellationToken ct = default) =>
        actual.GetUserApplicationRoleConnectionAsync(applicationID, ct);

    public Task<Result<IApplicationRoleConnection>> UpdateUserApplicationRoleConnectionAsync(Snowflake applicationID,
        Optional<string> platformName = default,
        Optional<string> platformUsername = default,
        Optional<IReadOnlyDictionary<string, string>> metadata = default,
        CancellationToken ct = default) =>
        actual.UpdateUserApplicationRoleConnectionAsync(applicationID, platformName, platformUsername, metadata, ct);

    public RestRequestCustomization WithCustomization(Action<RestRequestBuilder> requestCustomizer)
    {
        if (actual is not IRestCustomizable customizable)
            throw new NotImplementedException("Decorated type is not IRestCustomizable.");

        return customizable.WithCustomization(requestCustomizer);
    }

    public void RemoveCustomization(RestRequestCustomization customization) => (actual as IRestCustomizable)?.RemoveCustomization(customization);
}
