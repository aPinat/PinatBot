using Microsoft.EntityFrameworkCore;
using PinatBot.Data.Modules.Moderation;

namespace PinatBot.Data;

public class Database : DbContext
{
    public Database(DbContextOptions options) : base(options) { }

    public DbSet<GeneralLoggingConfig> GeneralLoggingConfigs => Set<GeneralLoggingConfig>();
    public DbSet<MemberJoinRoleConfig> MemberJoinRoleConfigs => Set<MemberJoinRoleConfig>();
    public DbSet<VoiceStateLoggingConfig> VoiceStateLoggingConfigs => Set<VoiceStateLoggingConfig>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new GeneralLoggingConfigEntityConfiguration().Configure(modelBuilder.Entity<GeneralLoggingConfig>());
        new MemberJoinRoleConfigEntityConfiguration().Configure(modelBuilder.Entity<MemberJoinRoleConfig>());
        new VoiceEventLoggingEntityConfiguration().Configure(modelBuilder.Entity<VoiceStateLoggingConfig>());
    }
}
