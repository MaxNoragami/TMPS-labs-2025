using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace lab2.MediaSources;

public class YoutubeSource : IMediaSource
{
    private static readonly HttpClient HttpClient = CreateHttpClient();

    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        var formattedUrl = FormatUrl(sourceUrl);

        const string formatSelector =

            "best[ext=mp4][vcodec!=none][acodec!=none]" +
            "[protocol!=m3u8][protocol!=m3u8_native]";

        var args = $"-f \"{formatSelector}\" --get-url {formattedUrl}";
        return await ExecuteYtDlpCommand(args);
    }


    public async Task<string> GetMetadataAsync(string sourceUrl)
    {
        var formattedUrl = FormatUrl(sourceUrl);
        var metadata = await ExecuteYtDlpCommand($"-j {formattedUrl}");
        return await UploadTo0x0(metadata, "metadata.json");
    }

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("lab2-media/1.0 (+https://example.com)");
        return client;
    }

    private async Task<string> UploadTo0x0(string content, string filename)
    {
        using var form = new MultipartFormDataContent();

        var bytes = Encoding.UTF8.GetBytes(content);
        var fileContent = new ByteArrayContent(bytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        form.Add(fileContent, "file", filename);

        var response = await HttpClient.PostAsync("https://0x0.st", form);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"0x0.st upload failed: {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                $"Response body: {body}");

        return body.Trim();
    }

    private async Task<string> ExecuteYtDlpCommand(string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(outputTask, errorTask);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"yt-dlp failed: {errorTask.Result}");

        return outputTask.Result.Trim();
    }

    private static string FormatUrl(string sourceUrl)
    {
        var cleanUrl = sourceUrl.Trim();

        if (!IsValidYouTubeUrl(cleanUrl))
            throw new ArgumentException("Invalid YouTube URL format");

        if (cleanUrl.Contains("youtu.be/"))
        {
            var videoId = ExtractVideoIdFromShortUrl(cleanUrl);
            return $"https://www.youtube.com/watch?v={videoId}";
        }

        return cleanUrl;
    }

    private static bool IsValidYouTubeUrl(string url)
    {
        return url.StartsWith("https://www.youtube.com/watch") ||
               url.StartsWith("https://youtube.com/watch") ||
               url.StartsWith("https://m.youtube.com/watch") ||
               url.StartsWith("https://youtu.be/") ||
               url.StartsWith("https://www.youtube.com/embed/") ||
               url.StartsWith("https://youtube.com/embed/");
    }

    private static string ExtractVideoIdFromShortUrl(string url)
    {
        var uri = new Uri(url);
        var videoId = uri.AbsolutePath.TrimStart('/');

        var queryIndex = videoId.IndexOf('?');
        if (queryIndex >= 0)
            videoId = videoId.Substring(0, queryIndex);

        return videoId;
    }
}
