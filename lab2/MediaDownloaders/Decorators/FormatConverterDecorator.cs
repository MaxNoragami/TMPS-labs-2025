using System.Diagnostics;

namespace lab2.MediaDownloaders.Decorators;

public class FormatConverterDecorator(
    IVideoDownloader downloader, string targetFormat) : VideoDownloaderDecorator(downloader)
{
    private readonly string _targetFormat = targetFormat;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var baseName = Path.GetFileNameWithoutExtension(outputPath);
        var dir = Path.GetDirectoryName(outputPath)!;
        var intermediatePath = Path.Combine(dir, $"{baseName}_converted.{_targetFormat}");

        await base.DownloadAsync(sourceUrl, intermediatePath);

        if (!string.Equals(intermediatePath, outputPath, StringComparison.OrdinalIgnoreCase))
        {
            await ConvertVideoAsync(intermediatePath, outputPath);
            File.Delete(intermediatePath);
        }
    }

    private async Task ConvertVideoAsync(string inputPath, string outputPath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -loglevel error -hide_banner -i \"{inputPath}\" \"{outputPath}\"",
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
        {
            throw new InvalidOperationException(
                $"ffmpeg failed with exit code {process.ExitCode}: {stderrTask.Result}");
        }
    }
}
