namespace lab2.MediaDownloaders.Proxies
{
    public class CachingVideoDownloaderProxy : IVideoDownloader
    {
        private readonly IVideoDownloader _downloader;
        private readonly string _cacheDirectory;

        public CachingVideoDownloaderProxy(IVideoDownloader downloader, string cacheDirectory = "cache")
        {
            _downloader = downloader;
            _cacheDirectory = cacheDirectory;

            CacheUtility.EnsureCacheDirectoryExists(_cacheDirectory);
        }

        public async Task DownloadAsync(string sourceUrl, string outputPath)
        {
            var cacheKey = CacheUtility.GenerateCacheKey(sourceUrl, outputPath);
            var cachedFilePath = Path.Combine(_cacheDirectory, cacheKey);

            if (File.Exists(cachedFilePath))
            {
                Console.WriteLine($"Cache hit! Copying from cache: {cachedFilePath}");
                File.Copy(cachedFilePath, outputPath, overwrite: true);
                Console.WriteLine($"Copied cached file to: {outputPath}");
                return;
            }

            Console.WriteLine($"Cache miss. Downloading from: {sourceUrl}");
            await _downloader.DownloadAsync(sourceUrl, outputPath);

            if (File.Exists(outputPath))
            {
                File.Copy(outputPath, cachedFilePath, overwrite: true);
                Console.WriteLine($"Cached file to: {cachedFilePath}");
            }
        }
    }
}