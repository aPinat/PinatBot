using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;

namespace PinatBot;

internal static class Extensions
{
    /// <summary>
    ///     Fisher–Yates shuffle.
    /// </summary>
    /// <param name="list">List to shuffle</param>
    /// <param name="rng"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this IList<T> list, Random rng)
    {
        var i = list.Count;
        while (i > 1)
        {
            i--;
            var j = rng.Next(i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    public static string Mention(this Snowflake id, Type type)
    {
        if (type.IsAssignableTo(typeof(IPartialUser)) || type.IsAssignableTo(typeof(IPartialGuildMember)))
            return $"<@{id}>";
        if (type.IsAssignableTo(typeof(IPartialChannel)))
            return $"<#{id}>";
        if (type.IsAssignableTo(typeof(IPartialRole)))
            return $"<@&{id}>";
        throw new ArgumentOutOfRangeException(nameof(type), type, "Type must implement IPartialUser, IPartialGuildMember, IPartialChannel or IPartialRole.");
    }

    public static string Mention(this IUser user) =>
        user.ID.Mention(user.GetType());

    public static string Mention(this IPartialUser user)
        => user.ID.IsDefined(out var id)
            ? id.Mention(user.GetType())
            : throw new ArgumentException("User has no ID.", nameof(user));

    public static string Mention(this IGuildMember member) =>
        member.User.IsDefined(out var user)
            ? user.ID.Mention(member.GetType())
            : throw new ArgumentException("Guild member has no user.", nameof(member));

    public static string Mention(this IPartialGuildMember member) =>
        member.User.IsDefined(out var user)
            ? user.ID.Mention(member.GetType())
            : throw new ArgumentException("Guild member has no user.", nameof(member));

    public static string Mention(this IChannel channel) =>
        channel.ID.Mention(channel.GetType());

    public static string Mention(this IPartialChannel channel) =>
        channel.ID.IsDefined(out var id)
            ? id.Mention(channel.GetType())
            : throw new ArgumentException("Channel has no ID.", nameof(channel));

    public static string Mention(this IRole role) =>
        role.ID.Mention(role.GetType());

    public static string Mention(this IPartialRole role) =>
        role.ID.IsDefined(out var id)
            ? id.Mention(role.GetType())
            : throw new ArgumentException("Role has no ID.", nameof(role));

    public static string Link(this IChannel channel) =>
        $"https://discord.com/channels/{channel.GuildID}/{channel.ID}";

    public static string Link(this IPartialChannel channel, Snowflake? guildId = null, Snowflake? channelId = null) =>
        $"https://discord.com/channels/{guildId ?? channel.GuildID.Value}/{channelId ?? channel.ID.Value}";

    public static string Link(this IMessage message) =>
        $"https://discord.com/channels/{message.GuildID}/{message.ChannelID}/{message.ID}";

    public static string Link(this IPartialMessage message, Snowflake? guildId = null, Snowflake? channelId = null, Snowflake? messageId = null) =>
        $"https://discord.com/channels/{guildId ?? message.GuildID.Value}/{channelId ?? message.ChannelID.Value}/{messageId ?? message.ID.Value}";

    public static string DiscordTag(this IUser user)
        => $"{user.Username}#{user.Discriminator:0000}";

    public static string DiscordTag(this IGuildMember member) =>
        member.User.IsDefined(out var user) ? user.DiscordTag() : throw new ArgumentException("Guild member has no user.", nameof(member));

    public static Uri AvatarUrl(this IUser user) =>
        user.Avatar is null ? CDN.GetDefaultUserAvatarUrl(user).Entity : CDN.GetUserAvatarUrl(user).Entity;

    public static Uri AvatarUrl(this IGuildMember member) =>
        member.User.IsDefined(out var user) ? user.AvatarUrl() : throw new ArgumentException("Guild member has no user.", nameof(member));

    /// <summary>
    ///     Converts a <see cref="DateTimeOffset" /> to Discord timestamp string.
    /// </summary>
    /// <param name="dateTimeOffset">Time to convert</param>
    /// <param name="format">R - Relative; d or D - Date; t or T - Time; f or F - Full</param>
    /// <returns>Discord timestamp string</returns>
    public static string ToDiscordTimestamp(this DateTimeOffset dateTimeOffset, char format = 'R')
        => $"<t:{dateTimeOffset.ToUnixTimeSeconds()}:{format}>";
}
