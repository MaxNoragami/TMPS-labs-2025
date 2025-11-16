namespace lab2.MediaSources;

public interface IMediaSource
{
    public Task<string> GetVideoAsync(string sourceUrl);
    public Task<string> GetAudioAsync(string sourceUrl);
    public Task<string> GetThumbnailAsync(string sourceUrl);
    public Task<Dictionary<string, object>> GetMetadataAsync(string sourceUrl);
}