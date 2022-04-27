using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class MemberJoinRoleResponder : IResponder<IGuildMemberAdd>
{
    public MemberJoinRoleResponder(MemberJoinRoleService memberJoinRoleService) => MemberJoinRoleService = memberJoinRoleService;
    private MemberJoinRoleService MemberJoinRoleService { get; }

    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => MemberJoinRoleService.GiveMemberJoinRoleAsync(member, ct);
}
