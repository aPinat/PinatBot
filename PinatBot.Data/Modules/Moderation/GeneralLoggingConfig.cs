using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinatBot.Data.Modules.Moderation;

public class GeneralLoggingConfig : LoggingConfig
{
    public GeneralLoggingConfig(ulong guildId) : base(guildId)
    {
    }
}

public class GeneralLoggingConfigEntityConfiguration : IEntityTypeConfiguration<GeneralLoggingConfig>
{
    public void Configure(EntityTypeBuilder<GeneralLoggingConfig> b)
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
