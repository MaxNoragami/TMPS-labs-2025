using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public abstract class MediaDownloader(IMediaSource mediaSource)
{
    protected IMediaSource mediaSource = mediaSource;


    public abstract Task DownloadAsync(string sourceUrl, string outputPath);

    protected virtual async Task<Dictionary<string, object>> GetMetadataAsync(string sourceUrl)
        => await mediaSource.GetMetadataAsync(sourceUrl);
}