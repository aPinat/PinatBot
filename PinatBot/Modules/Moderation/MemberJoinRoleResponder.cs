using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class MemberJoinRoleResponder(MemberJoinRoleService memberJoinRoleService) : IResponder<IGuildMemberAdd>
{
    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => memberJoinRoleService.GiveMemberJoinRoleAsync(member, ct);
}
