using Microsoft.EntityFrameworkCore;
using PinatBot.Data.Modules.AutoVoiceChannels;
using PinatBot.Data.Modules.Moderation;

namespace PinatBot.Data;

public class Database(DbContextOptions options) : DbContext(options)
{
    public DbSet<GeneralLoggingConfig> GeneralLoggingConfigs => Set<GeneralLoggingConfig>();
    public DbSet<MemberJoinRoleConfig> MemberJoinRoleConfigs => Set<MemberJoinRoleConfig>();
    public DbSet<VoiceStateLoggingConfig> VoiceStateLoggingConfigs => Set<VoiceStateLoggingConfig>();

    public DbSet<AutoVoiceChannel> AutoVoiceChannels => Set<AutoVoiceChannel>();
    public DbSet<NewSessionChannel> NewSessionChannels => Set<NewSessionChannel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new GeneralLoggingConfigEntityConfiguration().Configure(modelBuilder.Entity<GeneralLoggingConfig>());
        new MemberJoinRoleConfigEntityConfiguration().Configure(modelBuilder.Entity<MemberJoinRoleConfig>());
        new VoiceEventLoggingEntityConfiguration().Configure(modelBuilder.Entity<VoiceStateLoggingConfig>());

        new AutoVoiceChannelEntityConfiguration().Configure(modelBuilder.Entity<AutoVoiceChannel>());
        new NewSessionChannelEntityConfiguration().Configure(modelBuilder.Entity<NewSessionChannel>());
    }
}
