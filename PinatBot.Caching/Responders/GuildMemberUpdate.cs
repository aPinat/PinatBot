using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Responders;
using Remora.Rest.Core;
using Remora.Results;

namespace PinatBot.Caching.Responders;

public class GuildMemberUpdate(DiscordGatewayCache cache) : IResponder<IGuildMemberUpdate>
{
    public Task<Result> RespondAsync(IGuildMemberUpdate m, CancellationToken ct = default)
    {
        cache.InternalUsers[m.User.ID.Value] = m.User;

        if (cache.InternalGuilds[m.GuildID.Value].MembersInternal.TryGetValue(m.User.ID.Value, out var member))
            member = new GuildMember(
                new Optional<IUser>(m.User),
                m.Nickname.IsDefined(out var nickname) ? nickname : member.Nickname,
                m.Avatar,
                m.Banner,
                m.Roles,
                m.JoinedAt ?? member.JoinedAt,
                m.PremiumSince.IsDefined(out var premiumSince) ? premiumSince : member.PremiumSince,
                m.IsDeafened.IsDefined(out var isDeafened) ? isDeafened : member.IsDeafened,
                m.IsMuted.IsDefined(out var isMuted) ? isMuted : member.IsMuted,
                member.Flags,
                m.IsPending.IsDefined(out var isPending) ? isPending : member.IsPending,
                member.Permissions,
                m.CommunicationDisabledUntil.IsDefined(out var communicationDisabledUntil) ? communicationDisabledUntil : member.CommunicationDisabledUntil
            );
        else
            member = new GuildMember(
                new Optional<IUser>(m.User),
                m.Nickname,
                m.Avatar,
                m.Banner,
                m.Roles,
                m.JoinedAt ?? DateTimeOffset.Now,
                m.PremiumSince,
                m.IsDeafened.IsDefined(out var isDeafened) && isDeafened,
                m.IsMuted.IsDefined(out var isMuted) && isMuted,
                default,
                m.IsPending.IsDefined(out var isPending) && isPending,
                default,
                m.CommunicationDisabledUntil
            );

        cache.InternalGuilds[m.GuildID.Value].MembersInternal[m.User.ID.Value] = member;
        return Task.FromResult(Result.FromSuccess());
    }
}
