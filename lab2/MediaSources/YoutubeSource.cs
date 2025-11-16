using System.Diagnostics;
using System.Text.Json;

namespace lab2.MediaSources;

public class YoutubeSource : IMediaSource
{
    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        return await ExecuteYtDlpCommand($"-f best --get-url {sourceUrl}");
    }

    public async Task<string> GetThumbnailAsync(string sourceUrl)
    {
        return await ExecuteYtDlpCommand($"--get-thumbnail {sourceUrl}");
    }

    private async Task<string> ExecuteYtDlpCommand(string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"yt-dlp failed: {error}");
        }

        return output.Trim();
    }
}