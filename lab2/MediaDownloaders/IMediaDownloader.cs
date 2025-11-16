namespace lab2.MediaDownloaders;

public interface IMediaDownloader
{
    public Task DownloadAsync(string sourceUrl, string outputPath);
}
