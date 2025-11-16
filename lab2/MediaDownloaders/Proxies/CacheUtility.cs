using System.Security.Cryptography;
using System.Text;

namespace lab2.MediaDownloaders.Proxies
{
    public static class CacheUtility
    {
        public static string ComputeHash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }

        public static string GenerateCacheKey(string url, string outputPath)
        {
            var extension = Path.GetExtension(outputPath);
            var hash = ComputeHash($"{url}_{extension}");
            return $"{hash}{extension}";
        }

        public static void EnsureCacheDirectoryExists(string cacheDirectory)
        {
            if (!Directory.Exists(cacheDirectory))
                Directory.CreateDirectory(cacheDirectory);
        }

        public static void ClearCache(string cacheDirectory)
        {
            if (Directory.Exists(cacheDirectory))
            {
                Directory.Delete(cacheDirectory, recursive: true);
                Directory.CreateDirectory(cacheDirectory);
                Console.WriteLine($"Cache cleared: {cacheDirectory}");
            }
        }

        public static void ClearCacheForUrl(string cacheDirectory, string url, string outputPath)
        {
            var cacheKey = GenerateCacheKey(url, outputPath);
            var cachedFilePath = Path.Combine(cacheDirectory, cacheKey);

            if (File.Exists(cachedFilePath))
            {
                File.Delete(cachedFilePath);
                Console.WriteLine($"Cleared cache for: {url}");
            }
        }
    }
}