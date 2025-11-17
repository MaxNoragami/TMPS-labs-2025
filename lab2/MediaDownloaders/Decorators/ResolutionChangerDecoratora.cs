using System.Diagnostics;

namespace lab2.MediaDownloaders.Decorators;

public class ResolutionChangerDecorator(
    IVideoDownloader downloader, string resolution) : VideoDownloaderDecorator(downloader)
{
    private readonly string _resolution = resolution;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}{Path.GetExtension(outputPath)}");

        try
        {
            await base.DownloadAsync(sourceUrl, tempFile);
            Console.WriteLine($"Starting resize to width {_resolution}...");
            await ResizeVideoAsync(tempFile, outputPath);
            Console.WriteLine($"Resize to width {_resolution} finished!");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private async Task ResizeVideoAsync(string inputPath, string outputPath)
    {
        if (!int.TryParse(_resolution, out var width) || width <= 0)
            throw new ArgumentException($"Invalid width: {_resolution}");

        var evenWidth = (width % 2 == 0) ? width : width + 1;

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -loglevel error -hide_banner -i \"{inputPath}\" -vf \"scale={evenWidth}:-2\" \"{outputPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();

        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(stdoutTask, stderrTask);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"ffmpeg failed with exit code {process.ExitCode}: {stderrTask.Result}");
    }
}
