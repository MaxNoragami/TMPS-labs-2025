using System.Text.Json;

namespace lab2.MediaSources;

public class RedditSource : IMediaSource
{
    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        var jsonUrl = FormatUrl(sourceUrl);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36");

        var jsonResponse = await httpClient.GetStringAsync(jsonUrl);
        var jsonDocument = JsonDocument.Parse(jsonResponse);

        var postData = jsonDocument.RootElement[0]
            .GetProperty("data")
            .GetProperty("children")[0]
            .GetProperty("data");

        if (postData.TryGetProperty("secure_media", out var secureMedia) &&
            secureMedia.TryGetProperty("reddit_video", out var redditVideo) &&
            redditVideo.TryGetProperty("fallback_url", out var fallbackUrl))
        {
            return fallbackUrl.GetString() ?? throw new InvalidOperationException("No video URL found");
        }

        if (postData.TryGetProperty("media", out var media) &&
            media.TryGetProperty("reddit_video", out var mediaRedditVideo) &&
            mediaRedditVideo.TryGetProperty("fallback_url", out var mediaFallbackUrl))
        {
            return mediaFallbackUrl.GetString() ?? throw new InvalidOperationException("No video URL found");
        }

        throw new InvalidOperationException("No Reddit video found in this post");
    }

    public async Task<string> GetMetadataAsync(string sourceUrl)
        => FormatUrl(sourceUrl);

    private static string FormatUrl(string sourceUrl)
    {
        var cleanUrl = sourceUrl.TrimEnd('/');

        if (cleanUrl.StartsWith("https://www.reddit.com"))
            cleanUrl = cleanUrl.Replace("https://www.reddit.com", "https://old.reddit.com");
        else if (cleanUrl.StartsWith("https://reddit.com"))
            cleanUrl = cleanUrl.Replace("https://reddit.com", "https://old.reddit.com");
        else if (!cleanUrl.StartsWith("https://old.reddit.com"))
            throw new ArgumentException("Invalid Reddit URL format");

        return $"{cleanUrl}.json";
    }
}