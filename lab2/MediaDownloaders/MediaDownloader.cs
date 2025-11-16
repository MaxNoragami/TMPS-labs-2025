using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public abstract class MediaDownloader(IMediaSource mediaSource) : IMediaDownloader
{
    protected IMediaSource mediaSource = mediaSource;

    public abstract Task DownloadAsync(string sourceUrl, string outputPath);
}