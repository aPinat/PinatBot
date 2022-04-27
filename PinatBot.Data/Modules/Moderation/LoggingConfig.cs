namespace PinatBot.Data.Modules.Moderation;

public class LoggingConfig
{
    protected LoggingConfig(ulong guildId) => GuildId = guildId;

    public ulong GuildId { get; }
    public bool Enabled { get; set; }
    public ulong ChannelId { get; set; }
}
