using System.Security.Cryptography;
using System.Text;

namespace lab2.MediaDownloaders.Proxies;

public static class CacheUtility
{
    public static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static string GenerateCacheKey(string url)
    {
        var hash = ComputeHash($"{url}");
        return $"{hash}.mp4";
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

    public static void ClearCacheForUrl(string cacheDirectory, string url)
    {
        var cacheKey = GenerateCacheKey(url);
        var cachedFilePath = Path.Combine(cacheDirectory, cacheKey);

        if (File.Exists(cachedFilePath))
        {
            File.Delete(cachedFilePath);
            Console.WriteLine($"Cleared cache for: {url}");
        }
    }
}
