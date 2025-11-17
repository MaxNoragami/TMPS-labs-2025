using lab2.Exceptions;
using lab2.MediaDownloaders;
using lab2.MediaDownloaders.Decorators;
using lab2.MediaDownloaders.Proxies;
using lab2.MediaSources;
using lab2.Utils;

void WaitAndClear(string message = "\nPress Enter to continue...")
{
    Console.WriteLine(message);
    Console.ReadLine();
    Console.Clear();
}

while (true)
{
    try
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        if (Directory.Exists(tempDir))
            Directory.Delete(tempDir, true);
        Directory.CreateDirectory(tempDir);

        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        Console.Clear();
        var sourceChoice = Input.ReadMenuChoice("Choose source:", "YouTube", "Reddit");
        IMediaSource source = sourceChoice == 1 ? new YoutubeSource() : new RedditSource();

        var url = Input.ReadNonEmpty("Enter URL: ");
        var outputBase = Input.ReadNonEmpty("Enter output filename (without extension): ");

        IVideoDownloader downloader = new CachingDownloaderProxy(
            new VideoDownloader(source), "cache/videos");

        string? targetFormat = null;

        while (true)
        {
            var step = Input.ReadMenuChoice(
                "\nAdd processing step?",
                "Format conversion",
                "Watermark",
                "Resize width",
                "Done"
            );
            if (step == 4) break;
            switch (step)
            {
                case 1:
                    targetFormat = Input.ReadFormat();
                    downloader = new FormatConverterDecorator(downloader, targetFormat);
                    break;
                case 2:
                    downloader = new WatermarkDecorator(downloader, Input.ReadWatermarkPath());
                    break;
                case 3:
                    downloader = new ResolutionChangerDecorator(downloader, Input.ReadWidth());
                    break;
            }
        }

        var finalExtension = targetFormat ?? "mp4";
        var output = Path.Combine(outputDir, $"{outputBase}.{finalExtension}");

        Console.Clear();
        Console.WriteLine("Downloading...");

        await downloader.DownloadAsync(url, output);

        if (Input.ReadYesNo("\nDownload metadata as well? (y/n): "))
        {
            var metadata = new CachingDownloaderProxy(
                new MetadataDownloader(source), "cache/metadata");
            var metaOut = Path.Combine(outputDir, Input.GetMetadataOutputFile());
            Console.Clear();
            Console.WriteLine("Downloading metadata...");
            await metadata.DownloadAsync(url, metaOut);
        }

        WaitAndClear("\nDone! Press 'Enter' to start over or 'Ctrl+C' to exit.");
    }
    catch (NoVideoFoundException ex)
    {
        Console.WriteLine($"\n! Error: {ex.Message}");
        WaitAndClear();
    }
    catch (InvalidResourceException ex)
    {
        Console.WriteLine($"\n! Error: {ex.Message}");
        WaitAndClear();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n! Unexpected error: {ex.Message}");
        WaitAndClear();
    }
}