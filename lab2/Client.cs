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
    Console.Clear();
    var sourceChoice = Input.ReadMenuChoice("Choose source:", "YouTube", "Reddit");
    IMediaSource source = sourceChoice == 1 ? new YoutubeSource() : new RedditSource();

    var url = Input.ReadNonEmpty("Enter URL: ");
    var outputBase = Input.ReadNonEmpty("Enter output filename (without extension): ");

    IVideoDownloader downloader = new CachingVideoDownloaderProxy(
        new VideoDownloader(source), "cache/videos");

    bool hasProcessing = false;
    string? targetFormat = null;

    while (true)
    {
        var step = Input.ReadMenuChoice(
            "\nAdd processing step?",
            "Format conversion",
            "Watermark",
            "Resolution",
            "Done"
        );
        if (step == 4) break;
        hasProcessing = true;
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
                downloader = new ResolutionChangerDecorator(downloader, Input.ReadResolution());
                break;
        }
    }

    var finalExtension = targetFormat ?? "mp4";
    var output = $"{outputBase}.{finalExtension}";

    Console.Clear();
    Console.WriteLine("Downloading...");

    await downloader.DownloadAsync(url, output);

    if (Input.ReadYesNo("\nDownload metadata as well? (y/n): "))
    {
        var metadata = new CachingMetadataDownloaderProxy(new MetadataDownloader(source), "cache/metadata");
        var metaOut = Input.GetMetadataOutputFile();
        Console.Clear();
        Console.WriteLine("Downloading metadata...");
        await metadata.DownloadAsync(url, metaOut);
    }

    WaitAndClear("\nDone! Press 'Enter' to start over or 'Ctrl+C' to exit.");
}