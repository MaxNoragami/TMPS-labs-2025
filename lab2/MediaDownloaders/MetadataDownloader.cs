using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public class MetadataDownloader(IMediaSource mediaSource) : MediaDownloader(mediaSource)
{
    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var metadataUrl = await mediaSource.GetMetadataAsync(sourceUrl);
        Console.WriteLine(metadataUrl);
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36");

        var jsonData = await httpClient.GetStringAsync(metadataUrl);
        await File.WriteAllTextAsync(outputPath, jsonData);

        Console.WriteLine($"Downloaded metadata to: {outputPath}");
    }
}