using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class UserUpdate : IResponder<IUserUpdate>
{
    private readonly DiscordGatewayCache _cache;

    public UserUpdate(DiscordGatewayCache cache) => _cache = cache;

    public Task<Result> RespondAsync(IUserUpdate u, CancellationToken ct = default)
    {
        _cache.CurrentUser = u;
        _cache.InternalUsers[u.ID.Value] = u;
        return Task.FromResult(Result.FromSuccess());
    }
}
