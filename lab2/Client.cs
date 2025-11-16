using lab2.MediaDownloaders;
using lab2.MediaDownloaders.Decorators;
using lab2.MediaSources;

// --- YouTube Downloads ---

// Plain download
var ytDownloader = new VideoDownloader(new YoutubeSource());
await ytDownloader.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.mp4");

// Converted and resized
var ytProcessed = new ResolutionChangerDecorator(
    new FormatConverterDecorator(
        new WatermarkDecorator(ytDownloader, "watermark.jpg"),
        "mp4"
    ),
    "1920x1080"
);
await ytProcessed.DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang_1080p.mp4");

// Metadata
await new MetadataDownloader(new YoutubeSource())
    .DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.json");


// --- Reddit Downloads ---

// Uncomment to use Reddit source
/*
var redditDownloader = new VideoDownloader(new RedditSource());
await redditDownloader.DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Video.mp4");

var redditProcessed = new FormatConverterDecorator(redditDownloader, "mp4");
await redditProcessed.DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Converted.mp4");

await new MetadataDownloader(new RedditSource())
    .DownloadAsync("https://www.reddit.com/r/unixporn/comments/1oyqrrg/hyprland_rice_in_magenta_style/", "Reddit_Metadata.json");
*/