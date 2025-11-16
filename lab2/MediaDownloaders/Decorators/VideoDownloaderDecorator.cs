
namespace lab2.MediaDownloaders.Decorators;

public abstract class VideoDownloaderDecorator(IVideoDownloader downloader) : IVideoDownloader
{
    protected readonly IVideoDownloader _downloader = downloader;

    public virtual async Task DownloadAsync(string sourceUrl, string outputPath)
        => await _downloader.DownloadAsync(sourceUrl, outputPath);
}
