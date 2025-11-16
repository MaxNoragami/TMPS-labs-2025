using lab2.MediaDownloaders;
using lab2.MediaSources;

await new VideoDownloader(new YoutubeSource()).DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.webm");

await new MetadataDownloader(new YoutubeSource()).DownloadAsync("https://www.youtube.com/watch?v=sKTIFe2tlrU", "Sung_Kang.json");