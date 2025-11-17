using System.Diagnostics;

namespace lab2.MediaDownloaders.Decorators;

public class WatermarkDecorator(
    IVideoDownloader downloader, string watermarkPath) : VideoDownloaderDecorator(downloader)
{
    private readonly string _watermarkPath = watermarkPath;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}{Path.GetExtension(outputPath)}");

        try
        {
            await base.DownloadAsync(sourceUrl, tempFile);
            Console.WriteLine($"Starting watermarking with '{_watermarkPath}'...");
            await AddWatermarkAsync(tempFile, outputPath);
            Console.WriteLine("Watermarking finished!");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private async Task AddWatermarkAsync(string inputPath, string outputPath)
    {
        var ffmpegArgs =
            $"-y -loglevel error -hide_banner -i \"{inputPath}\" -i \"{_watermarkPath}\" " +
            "-filter_complex " +
            "\"[1:v][0:v]scale2ref=w=iw:h=ih:force_original_aspect_ratio=decrease[wm][vid];" +
            "[wm]format=rgba,colorchannelmixer=aa=0.3[wm2];" +
            "[vid][wm2]overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2\" " +
            $"\"{outputPath}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
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
