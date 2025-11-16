namespace lab2.MediaSources;

public interface IMediaSource
{
    public Task<string> GetVideoAsync(string sourceUrl);
    public Task<string> GetThumbnailAsync(string sourceUrl);
}