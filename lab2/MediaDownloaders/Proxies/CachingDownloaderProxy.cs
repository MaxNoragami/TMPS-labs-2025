namespace lab2.MediaDownloaders.Proxies;

public class CachingDownloaderProxy : IMediaDownloader, IVideoDownloader
{
    private readonly MediaDownloader _downloader;
    private readonly string _cacheDirectory;

    public CachingDownloaderProxy(
        MediaDownloader downloader, string cacheDirectory = "cache")
    {
        _downloader = downloader;
        _cacheDirectory = cacheDirectory;

        CacheUtility.EnsureCacheDirectoryExists(_cacheDirectory);
    }

    public async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var baseCacheKey = CacheUtility.GenerateCacheKey(sourceUrl);
        var cachedFilePath = Path.Combine(_cacheDirectory, baseCacheKey);

        if (File.Exists(cachedFilePath))
        {
            Console.WriteLine($"Cache hit! Using cached file: {cachedFilePath}");
            File.Copy(cachedFilePath, outputPath, overwrite: true);
            Console.WriteLine($"Copied cached file to: {outputPath}");
            return;
        }

        Console.WriteLine($"Cache miss. Downloading from: {sourceUrl}");

        var tempDownloadPath = Path.Combine("temp", $"{Guid.NewGuid()}.mp4");
        await _downloader.DownloadAsync(sourceUrl, tempDownloadPath);

        if (File.Exists(tempDownloadPath))
        {
            File.Copy(tempDownloadPath, cachedFilePath, overwrite: true);
            Console.WriteLine($"Cached base file to: {cachedFilePath}");

            File.Move(tempDownloadPath, outputPath, overwrite: true);
        }
    }
}
