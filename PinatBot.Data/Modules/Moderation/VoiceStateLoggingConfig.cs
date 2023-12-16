using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinatBot.Data.Modules.Moderation;

public class VoiceStateLoggingConfig(ulong guildId) : LoggingConfig(guildId);

public class VoiceEventLoggingEntityConfiguration : IEntityTypeConfiguration<VoiceStateLoggingConfig>
{
    public void Configure(EntityTypeBuilder<VoiceStateLoggingConfig> b)
    {
        b.Property(m => m.GuildId)
            .IsRequired()
            .ValueGeneratedNever();

        b.Property(m => m.Enabled)
            .IsRequired();

        b.Property(m => m.ChannelId)
            .IsRequired()
            .ValueGeneratedNever();

        b.HasKey(m => m.GuildId);

        b.HasIndex(m => m.GuildId)
            .IsUnique();
    }
}
