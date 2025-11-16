using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public class ThumbnailDownloader(IMediaSource mediaSource) : MediaDownloader(mediaSource)
{
    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var directThumbnailUrl = await mediaSource.GetThumbnailAsync(sourceUrl);

        using var httpClient = new HttpClient();
        var thumbnailBytes = await httpClient.GetByteArrayAsync(directThumbnailUrl);
        await File.WriteAllBytesAsync(outputPath, thumbnailBytes);

        Console.WriteLine($"Downloaded thumbnail to: {outputPath}");
    }
}