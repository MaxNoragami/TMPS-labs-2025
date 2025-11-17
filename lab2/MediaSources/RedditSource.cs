using System.Text.Json;
using lab2.Exceptions;
using lab2.MediaDownloaders.Extensions;

namespace lab2.MediaSources;

public class RedditSource : IMediaSource
{
    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        var jsonUrl = FormatUrl(sourceUrl);

        using var httpClient = new HttpClient().AddUserAgent();

        var jsonResponse = await httpClient.GetStringAsync(jsonUrl);
        var jsonDocument = JsonDocument.Parse(jsonResponse);

        var postData = jsonDocument.RootElement[0]
            .GetProperty("data")
            .GetProperty("children")[0]
            .GetProperty("data");

        if (postData.TryGetProperty("secure_media", out var secureMedia) &&
                secureMedia.ValueKind != JsonValueKind.Null &&
                secureMedia.TryGetProperty("reddit_video", out var redditVideo) &&
                redditVideo.TryGetProperty("fallback_url", out var fallbackUrl))
        {
            return fallbackUrl.GetString() ?? throw new NoVideoFoundException("Video URL is null");
        }

        // Check media as fallback
        if (postData.TryGetProperty("media", out var media) &&
            media.ValueKind != JsonValueKind.Null &&
            media.TryGetProperty("reddit_video", out var mediaRedditVideo) &&
            mediaRedditVideo.TryGetProperty("fallback_url", out var mediaFallbackUrl))
        {
            return mediaFallbackUrl.GetString() ?? throw new NoVideoFoundException("Video URL is null");
        }

        throw new NoVideoFoundException("This Reddit post does not contain a video");
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