using lab2.MediaSources;

namespace lab2.MediaDownloaders;

public class AudioDownloader(IMediaSource mediaSource) : MediaDownloader(mediaSource)
{
    public override Task DownloadAsync(string sourceUrl, string outputPath)
    {
        throw new NotImplementedException();
    }

    protected override async Task<Dictionary<string, object>> GetMetadataAsync(string sourceUrl)
    {
        var metadata = await base.GetMetadataAsync(sourceUrl);
        throw new NotImplementedException();
    }
}
