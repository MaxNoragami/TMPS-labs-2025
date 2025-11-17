namespace lab2.MediaDownloaders.Extensions;

public static class HttpClientExtension
{
    public static HttpClient AddUserAgent(this HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36");
        return httpClient;
    }

}