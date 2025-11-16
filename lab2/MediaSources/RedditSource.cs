using AngleSharp;

namespace lab2.MediaSources;

public class RedditSource : IMediaSource
{
    // public static async Task Test(string url)
    // {
    //     var config = Configuration.Default.WithDefaultLoader();
    //     var context = BrowsingContext.New(config);

    //     var document = await context.OpenAsync(url);

    //     var titleElement = document.QuerySelector("head>title");

    //     Console.WriteLine("Node Name: " + titleElement?.NodeName.ToLower() + "\n" + titleElement?.OuterHtml);
    // }
    public Task<string> GetAudioAsync(string sourceUrl)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, object>> GetMetadataAsync(string sourceUrl)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetThumbnailAsync(string sourceUrl)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetVideoAsync(string sourceUrl)
    {
        throw new NotImplementedException();
    }
}