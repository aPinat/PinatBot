namespace PinatBot;

public class Configuration
{
    public DiscordConfig Discord { get; init; } = null!;
    public LavalinkConfig Lavalink { get; init; } = null!;
    public ConnectionStrings ConnectionStrings { get; init; } = null!;
    public LeagueConfig League { get; init; } = null!;
}

public class DiscordConfig
{
    public string BotToken { get; init; } = null!;
    public IEnumerable<ulong> EmojiGuilds { get; init; } = null!;
    public string Prefix { get; init; } = null!;
    public ulong? TestGuild { get; init; } = null!;
}

public class LavalinkConfig
{
    public bool Enabled { get; init; } = false;
    public string Host { get; init; } = null!;
    public int Port { get; init; } = 2333;
    public string Password { get; init; } = null!;
}

public class ConnectionStrings
{
    public string Postgres { get; init; } = null!;
    public string Redis { get; init; } = null!;
}

public class LeagueConfig
{
    public string RiotApiKey { get; init; } = null!;
    public string? RiotApiKeyExtra { get; init; } = null;
    public bool LiveGameMonitorEnabled { get; init; } = true;
}
