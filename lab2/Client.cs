using lab2.MediaDownloaders;
using lab2.MediaDownloaders.Decorators;
using lab2.MediaDownloaders.Proxies;
using lab2.MediaSources;


// Plain download with cache
var ytDownloader = new VideoDownloader(new YoutubeSource());
var cachedDownloader = new CachingVideoDownloaderProxy(ytDownloader, "cache/videos");
await cachedDownloader.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.mp4");

// Converted, watermarked, resized, with cache
var ytProcessed = new ResolutionChangerDecorator(
    new FormatConverterDecorator(
        new WatermarkDecorator(ytDownloader, "watermark.jpg"),
        "mp4"
    ),
    "1920x1080"
);
var cachedProcessed = new CachingVideoDownloaderProxy(ytProcessed, "cache/processed");
await cachedProcessed.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang_1080p.mp4");

// Metadata with cache
var metadataDownloader = new MetadataDownloader(new YoutubeSource());
var cachedMetadata = new CachingMetadataDownloaderProxy(metadataDownloader, "cache/metadata");
await cachedMetadata.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.json");

// Test cache hit: download again, should copy from cache
await cachedDownloader.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang_copy.mp4");
await cachedProcessed.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang_1080p_copy.mp4");
await cachedMetadata.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang_copy.json");



var redditDownloader = new VideoDownloader(new RedditSource());
var cachedReddit = new CachingVideoDownloaderProxy(redditDownloader, "cache/reddit");
await cachedReddit.DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Video.mp4");

var redditProcessed = new FormatConverterDecorator(redditDownloader, "mp4");
var cachedRedditProcessed = new CachingVideoDownloaderProxy(redditProcessed, "cache/reddit_processed");
await cachedRedditProcessed.DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Converted.mp4");

var redditMetadata = new MetadataDownloader(new RedditSource());
var cachedRedditMetadata = new CachingMetadataDownloaderProxy(redditMetadata, "cache/reddit_metadata");
await cachedRedditMetadata.DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Metadata.json");
