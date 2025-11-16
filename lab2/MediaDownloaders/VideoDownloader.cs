using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public class VideoDownloader(IMediaSource mediaSource) : MediaDownloader(mediaSource), IVideoDownloader
{
    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var directVideoUrl = await mediaSource.GetVideoAsync(sourceUrl);

        using var httpClient = new HttpClient();
        var videoBytes = await httpClient.GetByteArrayAsync(directVideoUrl);
        await File.WriteAllBytesAsync(outputPath, videoBytes);

        Console.WriteLine($"Downloaded video to: {outputPath}");
    }
}
