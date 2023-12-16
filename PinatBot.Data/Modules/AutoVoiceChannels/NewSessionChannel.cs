using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PinatBot.Data.Modules.AutoVoiceChannels;

public class NewSessionChannel(ulong channelId, string childName)
{
    public ulong ChannelId { get; } = channelId;
    public string ChildName { get; set; } = childName;
}

public class NewSessionChannelEntityConfiguration : IEntityTypeConfiguration<NewSessionChannel>
{
    public void Configure(EntityTypeBuilder<NewSessionChannel> b)
    {
        b.Property(m => m.ChannelId)
            .IsRequired()
            .ValueGeneratedNever();

        b.Property(m => m.ChildName)
            .IsRequired();

        b.HasKey(m => m.ChannelId);

        b.HasIndex(m => m.ChannelId)
            .IsUnique();
    }
}
