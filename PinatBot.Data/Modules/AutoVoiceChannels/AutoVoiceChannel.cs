using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinatBot.Data.Modules.AutoVoiceChannels;

public class AutoVoiceChannel(ulong channelId)
{
    public ulong ChannelId { get; } = channelId;
}

public class AutoVoiceChannelEntityConfiguration : IEntityTypeConfiguration<AutoVoiceChannel>
{
    public void Configure(EntityTypeBuilder<AutoVoiceChannel> b)
    {
        b.Property(m => m.ChannelId)
            .IsRequired()
            .ValueGeneratedNever();

        b.HasKey(m => m.ChannelId);

        b.HasIndex(m => m.ChannelId)
            .IsUnique();
    }
}
