using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace PinatBot.Modules.Moderation;

public class MemberJoinRoleResponder : IResponder<IGuildMemberAdd>
{
    private readonly MemberJoinRoleService _memberJoinRoleService;
    public MemberJoinRoleResponder(MemberJoinRoleService memberJoinRoleService) => _memberJoinRoleService = memberJoinRoleService;


    public Task<Result> RespondAsync(IGuildMemberAdd member, CancellationToken ct = default) => _memberJoinRoleService.GiveMemberJoinRoleAsync(member, ct);
}
