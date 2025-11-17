namespace lab2.MediaDownloaders.Proxies;

public class CachingDownloaderProxy : IVideoDownloader, IMediaDownloader
{
    private readonly IMediaDownloader _downloader;
    private readonly string _cacheDirectory;

    public CachingDownloaderProxy(
        IMediaDownloader downloader, string cacheDirectory = "cache")
    {
        _downloader = downloader;
        _cacheDirectory = cacheDirectory;

        CacheUtility.EnsureCacheDirectoryExists(_cacheDirectory);
    }

    public async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        // Always use base cache key (url only, always .mp4)
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

        // Download to temp location first (always mp4)
        var tempDownloadPath = Path.Combine("temp", $"{Guid.NewGuid()}.mp4");
        await _downloader.DownloadAsync(sourceUrl, tempDownloadPath);

        if (File.Exists(tempDownloadPath))
        {
            // Cache the base mp4 file
            File.Copy(tempDownloadPath, cachedFilePath, overwrite: true);
            Console.WriteLine($"Cached base file to: {cachedFilePath}");

            // Move to final output path (decorators handle conversion)
            File.Move(tempDownloadPath, outputPath, overwrite: true);
        }
    }
}
