namespace lab2.MediaSources;

public interface IMediaSource
{
    public Task<string> GetVideoAsync(string sourceUrl);
    public Task<string> GetMetadataAsync(string sourceUrl);

}