using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinatBot.Data.Modules.Moderation;

public class MemberJoinRoleConfig
{
    public MemberJoinRoleConfig(ulong guildId) => GuildId = guildId;

    public ulong GuildId { get; }
    public bool Enabled { get; set; }
    public ulong RoleId { get; set; }
}

public class MemberJoinRoleConfigEntityConfiguration : IEntityTypeConfiguration<MemberJoinRoleConfig>
{
    public void Configure(EntityTypeBuilder<MemberJoinRoleConfig> b)
    {
        b.Property(m => m.GuildId)
            .IsRequired()
            .ValueGeneratedNever();

        b.Property(m => m.Enabled)
            .IsRequired();

        b.Property(m => m.RoleId)
            .IsRequired()
            .ValueGeneratedNever();

        b.HasKey(m => m.GuildId);

        b.HasIndex(m => m.GuildId)
            .IsUnique();
    }
}
