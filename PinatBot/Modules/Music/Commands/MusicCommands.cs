using System.ComponentModel;
using Lavalink4NET;
using Lavalink4NET.Clients;
using Lavalink4NET.Players;
using Lavalink4NET.Players.Queued;
using Lavalink4NET.Remora.Discord;
using Lavalink4NET.Rest.Entities.Tracks;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Extensions.Formatting;
using Remora.Results;

namespace PinatBot.Modules.Music.Commands;

public class MusicCommands(IOperationContext operationContext, IFeedbackService feedbackService, IAudioService audioService) : CommandGroup
{
    [Command("play", "p")]
    [Description("Plays supplied URL or searches for specified keywords.")]
    public async Task<IResult> PlayAsync([Description("Terms to search for.")] [Greedy] string query)
    {
        var player = await GetPlayerAsync(connectToVoiceChannel: true);
        if (player is null)
            return Result.FromSuccess();

        if (Uri.TryCreate(query, UriKind.Absolute, out _))
        {
            var tracks = await audioService.Tracks.LoadTracksAsync(query, TrackSearchMode.None);
            if (tracks.IsPlaylist)
            {
                var playlist = tracks.Playlist;
                if (playlist is null)
                    goto NO_TRACKS;

                foreach (var track in tracks.Tracks)
                    await player.PlayAsync(track);

                return await feedbackService.SendContextualSuccessAsync(
                    $"Added playlist {Markdown.Bold(Markdown.Sanitize(playlist.Name))} ({tracks.Tracks.Length} tracks) to the playback queue.");
            }

            if (tracks.IsSuccess)
            {
                var track = tracks.Track;
                if (track is null)
                    goto NO_TRACKS;

                await player.PlayAsync(track);
                return await feedbackService.SendContextualSuccessAsync(
                    $"Added {Markdown.Bold(Markdown.Sanitize(track.Title))} by {Markdown.Bold(Markdown.Sanitize(track.Author))} ({track.Uri}) to the playback queue.");
            }
        }
        else
        {
            var track = await audioService.Tracks.LoadTrackAsync(query, TrackSearchMode.Deezer);
            if (track is null)
                goto NO_TRACKS;

            await player.PlayAsync(track);
            return await feedbackService.SendContextualSuccessAsync(
                $"Added {Markdown.Bold(Markdown.Sanitize(track.Title))} by {Markdown.Bold(Markdown.Sanitize(track.Author))} ({track.Uri}) to the playback queue.");
        }

        NO_TRACKS:
        return await feedbackService.SendContextualErrorAsync("No tracks were found for this search query.");
    }

    [Command("stop")]
    [Description("Stops playback and quits the voice channel.")]
    public async Task<IResult> StopAsync()
    {
        var player = await GetPlayerAsync();
        if (player is null)
            return Result.FromSuccess();

        var queueCount = player.Queue.Count;
        if (player.CurrentItem is not null)
            queueCount++;

        await player.StopAsync();
        await player.DisconnectAsync();

        return await feedbackService.SendContextualSuccessAsync($"Removed {queueCount} tracks from the queue.");
    }

    [Command("pause")]
    [Description("Pauses playback.")]
    public async Task<IResult> PauseAsync()
    {
        var player = await GetPlayerAsync();
        if (player is null)
            return Result.FromSuccess();

        await player.PauseAsync();
        return await feedbackService.SendContextualSuccessAsync($"Playback paused. Use {Markdown.InlineCode("/resume")} to resume playback.");
    }

    [Command("resume", "unpause")]
    [Description("Resumes playback.")]
    public async Task<IResult> ResumeAsync()
    {
        var player = await GetPlayerAsync();
        if (player is null)
            return Result.FromSuccess();

        await player.ResumeAsync();
        return await feedbackService.SendContextualSuccessAsync("Playback resumed.");
    }

    [Command("skip", "next")]
    [Description("Skips current track.")]
    public async Task<IResult> SkipAsync()
    {
        var player = await GetPlayerAsync();
        if (player is null)
            return Result.FromSuccess();

        var track = player.CurrentItem;
        if (track?.Track is null)
            return await feedbackService.SendContextualErrorAsync("No track is currently playing.");

        await player.SkipAsync();
        return await feedbackService.SendContextualSuccessAsync(
            $"{Markdown.Bold(Markdown.Sanitize(track.Track.Title))} by {Markdown.Bold(Markdown.Sanitize(track.Track.Author))} skipped.");
    }

    private async ValueTask<QueuedLavalinkPlayer?> GetPlayerAsync(bool connectToVoiceChannel = false)
    {
        var channelBehavior = connectToVoiceChannel
            ? PlayerChannelBehavior.Join
            : PlayerChannelBehavior.None;

        var retrieveOptions = new PlayerRetrieveOptions { ChannelBehavior = channelBehavior, VoiceStateBehavior = MemberVoiceStateBehavior.RequireSame };

        var result = await audioService.Players.RetrieveAsync(operationContext, PlayerFactory.Queued, retrieveOptions);

        if (result.IsSuccess)
            return result.Player;

        var errorMessage = result.Status switch
        {
            PlayerRetrieveStatus.UserNotInVoiceChannel => "You are not connected to a voice channel.",
            PlayerRetrieveStatus.BotNotConnected => "The bot is currently not connected.",
            _ => "Unknown error.",
        };

        await feedbackService.SendContextualErrorAsync(errorMessage);
        return null;
    }
}
