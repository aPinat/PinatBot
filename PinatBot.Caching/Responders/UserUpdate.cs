using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class UserUpdate(DiscordGatewayCache cache) : IResponder<IUserUpdate>
{
    public Task<Result> RespondAsync(IUserUpdate u, CancellationToken ct = default)
    {
        cache.CurrentUser = u;
        cache.InternalUsers[u.ID.Value] = u;
        return Task.FromResult(Result.FromSuccess());
    }
}
