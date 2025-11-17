# Laboratory Work #2: Structural Design Patterns

**Author:** Maxim Alexei, *FAF-232*


## Objectives

1. Study and understand the Structural Design Patterns.

2. As a continuation of the previous laboratory work, think about the functionalities that your system will need to provide to the user, or just implement a new small project.

3. Implement some additional functionalities using structural design patterns.

---

## Theory

In software engineering, structural design patterns are concerned with how classes and objects are composed to form larger structures. These patterns help ensure that when one part of a system changes, the entire structure doesn't need to change. They use inheritance to compose interfaces and define ways to compose objects to obtain new functionality.

### Adapter Pattern

The Adapter pattern allows objects with incompatible interfaces to collaborate. It acts as a wrapper between two objects, catching calls for one object and transforming them to a format and interface recognizable by the second object. The adapter implements the interface of one object and wraps the other one.

**Key characteristics:**
- Converts one interface to another
- Allows incompatible interfaces to work together
- Can wrap multiple adaptees with different interfaces
- Follows Single Responsibility Principle
- Follows Open/Closed Principle

### Bridge Pattern

The Bridge pattern divides a large class or a set of closely related classes into two separate hierarchies—abstraction and implementation—which can be developed independently of each other. It helps when you need to extend a class in several orthogonal (independent) dimensions.

**Key characteristics:**
- Separates abstraction from implementation
- Both hierarchies can be extended independently
- Implementation details hidden from clients
- Improved extensibility
- Allows runtime implementation switching

### Composite Pattern

The Composite pattern lets you compose objects into tree structures to represent part-whole hierarchies. It allows clients to treat individual objects and compositions of objects uniformly. The pattern defines a class hierarchy that contains both primitive and complex objects.

**Key characteristics:**
- Composes objects into tree structures
- Treats individual objects and compositions uniformly
- Makes it easier to add new kinds of components
- Can make design overly general
- Simplifies client code

### Decorator Pattern

The Decorator pattern lets you attach new behaviors to objects by placing these objects inside special wrapper objects that contain the behaviors. It provides a flexible alternative to subclassing for extending functionality, allowing responsibilities to be added to objects dynamically and transparently without affecting other objects.

**Key characteristics:**
- Adds responsibilities to objects dynamically
- Provides flexible alternative to subclassing
- Supports Open/Closed Principle
- Can wrap objects multiple times
- Each decorator is independent of others

### Facade Pattern

The Facade pattern provides a simplified interface to a library, framework, or any other complex set of classes. It defines a higher-level interface that makes the subsystem easier to use by wrapping a complicated subsystem with a simpler interface.

**Key characteristics:**
- Provides simple interface to complex subsystem
- Decouples client from subsystem components
- Promotes weak coupling
- Doesn't prevent applications from using subsystem classes
- Can become a god object coupled to all classes

### Flyweight Pattern

The Flyweight pattern lets you fit more objects into available RAM by sharing common parts of state between multiple objects instead of keeping all data in each object. The pattern achieves this by sharing the intrinsic state between objects while keeping the extrinsic state external.

**Key characteristics:**
- Reduces memory consumption
- Shares common state between objects
- Distinguishes intrinsic and extrinsic state
- Immutable flyweight objects
- May trade RAM for CPU cycles

### Proxy Pattern

The Proxy pattern provides a substitute or placeholder for another object. A proxy controls access to the original object, allowing you to perform something either before or after the request gets through to the original object. It's particularly useful for lazy initialization, access control, logging, or caching.

**Key characteristics:**
- Controls access to the original object
- Can perform additional operations transparently
- Implements same interface as the real subject
- Can delay expensive object creation
- Useful for caching, logging, and access control

---

## About the Project

For this laboratory work, I have implemented a **Media Downloader System** that downloads videos and metadata from various sources (YouTube, Reddit) with support for dynamic processing operations. The system demonstrates structural design patterns in a practical scenario involving media processing pipelines.

#### Some Features

The application allows users to:
- Download videos from multiple sources (YouTube, Reddit)
- Apply processing operations dynamically (format conversion, watermarking, resizing)
- Cache downloaded content for improved performance
- Download and save metadata alongside videos
- Chain multiple processing operations in any order

#### Architecture

The project is organized with clear separation of concerns:

1. **Media Sources** - Abstraction for different content providers
2. **Media Downloaders** - Core download functionality
3. **Decorators** - Dynamic processing operations
4. **Proxies** - Caching and access control
5. **Client** - User interaction and orchestration

#### Used Design Patterns

The implementation incorporates three structural design patterns:

1. **Bridge Pattern** - Separates download abstraction (`MediaDownloader`) from platform implementations (`IMediaSource`)

2. **Decorator Pattern** - Adds processing operations (format conversion, watermarking, resizing) dynamically to downloaders

3. **Proxy Pattern** - Implements caching mechanism to avoid redundant downloads

---

## Implementation

### 1. Bridge Pattern Implementation

The Bridge pattern is implemented by separating the download abstraction from the media source implementation, allowing both to vary independently.

<img src="https://files.catbox.moe/kkt0q2.png" alt="bridge-diagram" width=500>

#### Abstraction Side

```csharp
public interface IMediaDownloader
{
    public Task DownloadAsync(string sourceUrl, string outputPath);
}

public abstract class MediaDownloader(IMediaSource mediaSource) : IMediaDownloader
{
    protected IMediaSource mediaSource = mediaSource;

    public abstract Task DownloadAsync(string sourceUrl, string outputPath);
}
```

The `IMediaDownloader` interface defines the abstraction, while `MediaDownloader` maintains a reference to the implementation (`IMediaSource`).

#### Implementation Side

```csharp
public interface IMediaSource
{
    public Task<string> GetVideoAsync(string sourceUrl);
    public Task<string> GetMetadataAsync(string sourceUrl);
}
```

The `IMediaSource` interface represents the implementation hierarchy that can vary independently.

#### Concrete Abstraction - VideoDownloader

```csharp
public class VideoDownloader(IMediaSource mediaSource) : MediaDownloader(mediaSource), IVideoDownloader
{
    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var directVideoUrl = await mediaSource.GetVideoAsync(sourceUrl);

        using var httpClient = new HttpClient().AddUserAgent();

        var videoBytes = await httpClient.GetByteArrayAsync(directVideoUrl);
        await File.WriteAllBytesAsync(outputPath, videoBytes);

        Console.WriteLine($"Downloaded video to: {outputPath}");
    }
}
```

`VideoDownloader` implements the abstraction by delegating source-specific logic to `IMediaSource`.

#### Concrete Implementation - YoutubeSource

```csharp
public class YoutubeSource : IMediaSource
{
    private static readonly HttpClient HttpClient = CreateHttpClient();

    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        var formattedUrl = FormatUrl(sourceUrl);

        const string formatSelector =
            "best[ext=mp4][vcodec!=none][acodec!=none]" +
            "[protocol!=m3u8][protocol!=m3u8_native]";

        var args = $"-f \"{formatSelector}\" --get-url {formattedUrl}";
        return await ExecuteYtDlpCommand(args);
    }

    public async Task<string> GetMetadataAsync(string sourceUrl)
    {
        var formattedUrl = FormatUrl(sourceUrl);
        var metadata = await ExecuteYtDlpCommand($"-j {formattedUrl}");
        return await UploadTo0x0(metadata, "metadata.json");
    }

    // Helper methods omitted for brevity...
}
```

`YoutubeSource` provides YouTube-specific implementation using `yt-dlp` for extracting direct video URLs.

#### Concrete Implementation - RedditSource

```csharp
public class RedditSource : IMediaSource
{
    public async Task<string> GetVideoAsync(string sourceUrl)
    {
        var jsonUrl = FormatUrl(sourceUrl);

        using var httpClient = new HttpClient().AddUserAgent();
        var jsonResponse = await httpClient.GetStringAsync(jsonUrl);
        var jsonDocument = JsonDocument.Parse(jsonResponse);

        var postData = jsonDocument.RootElement[0]
            .GetProperty("data")
            .GetProperty("children")[0]
            .GetProperty("data");

        if (postData.TryGetProperty("secure_media", out var secureMedia) &&
            secureMedia.ValueKind != JsonValueKind.Null &&
            secureMedia.TryGetProperty("reddit_video", out var redditVideo) &&
            redditVideo.TryGetProperty("fallback_url", out var fallbackUrl))
        {
            return fallbackUrl.GetString() ?? throw new NoVideoFoundException("Video URL is null");
        }

        throw new NoVideoFoundException("This Reddit post does not contain a video");
    }

    public async Task<string> GetMetadataAsync(string sourceUrl)
        => FormatUrl(sourceUrl);

    // Helper methods omitted for brevity...
}
```

`RedditSource` provides Reddit-specific implementation by parsing Reddit's JSON API.

#### Usage in Client

```csharp
var sourceChoice = Input.ReadMenuChoice("Choose source:", "YouTube", "Reddit");
IMediaSource source = sourceChoice == 1 ? new YoutubeSource() : new RedditSource();

IVideoDownloader downloader = new VideoDownloader(source);
```

The client can switch between different media sources without changing the downloader code, demonstrating the power of the Bridge pattern.

### 2. Decorator Pattern Implementation

The Decorator pattern allows adding processing operations to the downloader dynamically without modifying its code.

<img src="https://files.catbox.moe/smgepd.png" alt="decorator-diagram" width=500>

#### Component Interface

```csharp
public interface IVideoDownloader : IMediaDownloader { }
```

The interface that both concrete components and decorators implement.

#### Base Decorator

```csharp
public abstract class VideoDownloaderDecorator(IVideoDownloader downloader) : IVideoDownloader
{
    protected readonly IVideoDownloader _downloader = downloader;

    public virtual async Task DownloadAsync(string sourceUrl, string outputPath)
        => await _downloader.DownloadAsync(sourceUrl, outputPath);
}
```

The base decorator maintains a reference to the wrapped component and delegates calls to it by default.

#### Concrete Decorator - FormatConverterDecorator

```csharp
public class FormatConverterDecorator(
    IVideoDownloader downloader, string targetFormat) : VideoDownloaderDecorator(downloader)
{
    private readonly string _targetFormat = targetFormat;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}.{_targetFormat}");

        try
        {
            await base.DownloadAsync(sourceUrl, tempFile);
            Console.WriteLine($"Starting conversion to {_targetFormat}...");
            await ConvertVideoAsync(tempFile, outputPath);
            Console.WriteLine($"Conversion to {_targetFormat} finished!");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private async Task ConvertVideoAsync(string inputPath, string outputPath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -loglevel error -hide_banner -i \"{inputPath}\" \"{outputPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();

        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(stdoutTask, stderrTask);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"ffmpeg failed with exit code {process.ExitCode}: {stderrTask.Result}");
    }
}
```

This decorator adds format conversion capability using FFmpeg. It downloads to a temporary file, converts it, then saves to the final output path.

#### Concrete Decorator - WatermarkDecorator

```csharp
public class WatermarkDecorator(
    IVideoDownloader downloader, string watermarkPath) : VideoDownloaderDecorator(downloader)
{
    private readonly string _watermarkPath = watermarkPath;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}{Path.GetExtension(outputPath)}");

        try
        {
            await base.DownloadAsync(sourceUrl, tempFile);
            Console.WriteLine($"Starting watermarking with '{_watermarkPath}'...");
            await AddWatermarkAsync(tempFile, outputPath);
            Console.WriteLine("Watermarking finished!");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private async Task AddWatermarkAsync(string inputPath, string outputPath)
    {
        var ffmpegArgs =
            $"-y -loglevel error -hide_banner -i \"{inputPath}\" -i \"{_watermarkPath}\" " +
            "-filter_complex " +
            "\"[1:v][0:v]scale2ref=w=iw:h=ih:force_original_aspect_ratio=decrease[wm][vid];" +
            "[wm]format=rgba,colorchannelmixer=aa=0.3[wm2];" +
            "[vid][wm2]overlay=(main_w-overlay_w)/2:(main_h-overlay_h)/2\" " +
            $"\"{outputPath}\"";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();

        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(stdoutTask, stderrTask);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"ffmpeg failed with exit code {process.ExitCode}: {stderrTask.Result}");
    }
}
```

This decorator adds watermark capability by overlaying an image on the video using FFmpeg's complex filter.

#### Concrete Decorator - ResolutionChangerDecorator

```csharp
public class ResolutionChangerDecorator(
    IVideoDownloader downloader, string resolution) : VideoDownloaderDecorator(downloader)
{
    private readonly string _resolution = resolution;

    public override async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        var tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
        var tempFile = Path.Combine(tempDir, $"{Guid.NewGuid()}{Path.GetExtension(outputPath)}");

        try
        {
            await base.DownloadAsync(sourceUrl, tempFile);
            Console.WriteLine($"Starting resize to width {_resolution}...");
            await ResizeVideoAsync(tempFile, outputPath);
            Console.WriteLine($"Resize to width {_resolution} finished!");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private async Task ResizeVideoAsync(string inputPath, string outputPath)
    {
        if (!int.TryParse(_resolution, out var width) || width <= 0)
            throw new ArgumentException($"Invalid width: {_resolution}");

        var evenWidth = (width % 2 == 0) ? width : width + 1;

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -loglevel error -hide_banner -i \"{inputPath}\" -vf \"scale={evenWidth}:-2\" \"{outputPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        process.Start();

        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(stdoutTask, stderrTask);
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"ffmpeg failed with exit code {process.ExitCode}: {stderrTask.Result}");
    }
}
```

This decorator adds video resizing capability while maintaining aspect ratio.

#### Dynamic Decoration in Client

```csharp
IVideoDownloader downloader = new CachingDownloaderProxy(
    new VideoDownloader(source), "cache/videos");

string? targetFormat = null;

while (true)
{
    var step = Input.ReadMenuChoice(
        "\nAdd processing step?",
        "Format conversion",
        "Watermark",
        "Resize width",
        "Done"
    );
    if (step == 4) break;
    switch (step)
    {
        case 1:
            targetFormat = Input.ReadFormat();
            downloader = new FormatConverterDecorator(downloader, targetFormat);
            break;
        case 2:
            downloader = new WatermarkDecorator(downloader, Input.ReadWatermarkPath());
            break;
        case 3:
            downloader = new ResolutionChangerDecorator(downloader, Input.ReadWidth());
            break;
    }
}
```

The client builds a processing pipeline by wrapping the downloader with multiple decorators. Each decorator adds its behavior independently, and they can be combined in any order.

### 3. Proxy Pattern Implementation

The Proxy pattern is implemented through a caching proxy that intercepts download requests and serves cached content when available.

<img src="https://files.catbox.moe/3c9x98.png" alt="proxy-diagram" width=500>

#### Subject Interface

```csharp
public interface IMediaDownloader
{
    public Task DownloadAsync(string sourceUrl, string outputPath);
}
```

Both the real subject and proxy implement this interface.

#### Caching Proxy

```csharp
public class CachingDownloaderProxy : IVideoDownloader, IMediaDownloader
{
    private readonly IMediaDownloader _downloader;
    private readonly string _cacheDirectory;

    public CachingDownloaderProxy(
        IMediaDownloader downloader, string cacheDirectory = "cache")
    {
        _downloader = downloader;
        _cacheDirectory = cacheDirectory;

        CacheUtility.EnsureCacheDirectoryExists(_cacheDirectory);
    }

    public async Task DownloadAsync(string sourceUrl, string outputPath)
    {
        // Always use base cache key (url only, always .mp4)
        var baseCacheKey = CacheUtility.GenerateCacheKey(sourceUrl);
        var cachedFilePath = Path.Combine(_cacheDirectory, baseCacheKey);

        if (File.Exists(cachedFilePath))
        {
            Console.WriteLine($"Cache hit! Using cached file: {cachedFilePath}");
            File.Copy(cachedFilePath, outputPath, overwrite: true);
            Console.WriteLine($"Copied cached file to: {outputPath}");
            return;
        }

        Console.WriteLine($"Cache miss. Downloading from: {sourceUrl}");

        // Download to temp location first (always mp4)
        var tempDownloadPath = Path.Combine("temp", $"{Guid.NewGuid()}.mp4");
        await _downloader.DownloadAsync(sourceUrl, tempDownloadPath);

        if (File.Exists(tempDownloadPath))
        {
            // Cache the base mp4 file
            File.Copy(tempDownloadPath, cachedFilePath, overwrite: true);
            Console.WriteLine($"Cached base file to: {cachedFilePath}");

            // Move to final output path (decorators handle conversion)
            File.Move(tempDownloadPath, outputPath, overwrite: true);
        }
    }
}
```

The proxy intercepts download requests, checks the cache first, and only delegates to the real downloader on cache misses. It caches the base MP4 file before any decorator transformations are applied.

#### Cache Utility

```csharp
public static class CacheUtility
{
    public static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }

    public static string GenerateCacheKey(string url)
    {
        var hash = ComputeHash($"{url}");
        return $"{hash}.mp4";
    }

    public static void EnsureCacheDirectoryExists(string cacheDirectory)
    {
        if (!Directory.Exists(cacheDirectory))
            Directory.CreateDirectory(cacheDirectory);
    }

    public static void ClearCache(string cacheDirectory)
    {
        if (Directory.Exists(cacheDirectory))
        {
            Directory.Delete(cacheDirectory, recursive: true);
            Directory.CreateDirectory(cacheDirectory);
            Console.WriteLine($"Cache cleared: {cacheDirectory}");
        }
    }

    public static void ClearCacheForUrl(string cacheDirectory, string url)
    {
        var cacheKey = GenerateCacheKey(url);
        var cachedFilePath = Path.Combine(cacheDirectory, cacheKey);

        if (File.Exists(cachedFilePath))
        {
            File.Delete(cachedFilePath);
            Console.WriteLine($"Cleared cache for: {url}");
        }
    }
}
```

The utility provides helper methods for cache key generation using SHA256 hashing, ensuring consistent cache keys based on URLs alone.

#### Usage in Client

```csharp
IVideoDownloader downloader = new CachingDownloaderProxy(
    new VideoDownloader(source), "cache/videos");

// Add decorators...
downloader = new FormatConverterDecorator(downloader, "mov");
downloader = new WatermarkDecorator(downloader, "watermark.png");

await downloader.DownloadAsync(url, output);
```

The proxy is wrapped around the base downloader before any decorators are applied. This ensures:
- Cache stores only base MP4 files
- Decorators operate on cached or freshly downloaded content
- Subsequent requests with different decorators reuse cached base files


### Pattern Integration

The three patterns work together to create a flexible and efficient media processing system:

1. **Bridge** separates download logic from platform-specific implementations
2. **Decorator** adds processing operations dynamically in any combination
3. **Proxy** optimizes performance through transparent caching

Example minimal workflow:
```csharp
// Bridge: Choose platform implementation
IMediaSource source = new YoutubeSource();

// Proxy: Add caching capability
IVideoDownloader downloader = new CachingDownloaderProxy(
    new VideoDownloader(source), "cache/videos");

// Decorator: Add processing operations dynamically
downloader = new FormatConverterDecorator(downloader, "webm");
downloader = new WatermarkDecorator(downloader, "logo.png");
downloader = new ResolutionChangerDecorator(downloader, "1280");

// Execute - cache is checked, decorators applied in order
await downloader.DownloadAsync(url, output);
```

---

## Results

The following screenshots demonstrate the functionality of the implemented Media Downloader System.

<img src="https://files.catbox.moe/nq5lod.png" alt="Being prompted to choose a source, either YouTube OR Reddit" width=500>

The application starts by prompting the user to select a media source, demonstrating the Bridge pattern's ability to switch between different platform implementations.

<img src="https://files.catbox.moe/ekz2q3.png" alt="Choosing YT, entering the URL, setting the file name as genshin_trailer, choosing to process the file by adding a watermark.png, resizing the width to 240, format to webm" width=500>

User selects YouTube, enters a video URL, specifies output filename, and dynamically adds multiple decorators, those being watermark, resize, format conversion, to build a custom processing pipeline.

<img src="https://files.catbox.moe/hlgf0a.png" alt="After hitting option 4 - Done, we wait for file to be downloaded or taken from cache if present, in this case it is not, the file gets downloaded to temp then moved to cache, then the post processing is applied, watermak, then resize, then conversion to webm, then we get prompted if we want to save the metadata" width=500>

Cache miss occurs, video downloads to temp, gets cached as base MP4, then decorators sequentially apply watermark, resize, and format conversion. User is prompted for metadata download.

<img src="https://files.catbox.moe/85rhn2.png" alt="After saying yes to saving the metadata, we wait for it to be downloaded as it is not present in cache, and that is it, we can either exit via ctrl+c or hit enter to start over" width=500>

Metadata is downloaded and saved to the output directory, completing the workflow.

<img src="https://files.catbox.moe/1p4f64.png" alt="viewing the output video through VLC, it works, all post processing were applied" width=500>

The output video displays in VLC player, confirming all decorator transformations, those being watermark, resize, format conversion, were successfully applied.

<img src="https://files.catbox.moe/414hwc.png" alt="viewing the metadata" width=500>

The downloaded metadata JSON file contains detailed video information retrieved from YouTube.

<img src="https://files.catbox.moe/9l7xz9.png" alt="trying to re-process a video off Reddit that was already cached, filling it url, and selected processing actions" width=500>

Switching to Reddit source, Bridge pattern, user enters a previously downloaded URL and selects different processing operations to demonstrate cache reusability.

<img src="https://files.catbox.moe/qdgxiu.png" alt="it instantly retrieved the video from cache and got to applying the desired processing behaviors" width=500>

Cache hit occurs—the Proxy pattern instantly retrieves the base MP4 from cache, then applies the new decorator operations without re-downloading.

---

## Conclusions

Through this laboratory work, I successfully demonstrated the implementation and integration of three fundamental structural design patterns in a practical media processing application.

The *Bridge pattern* proved essential for decoupling the download abstraction from platform-specific implementations. By separating `MediaDownloader` from `IMediaSource`, the system can easily support new platforms (Twitter, Instagram, etc.) without modifying existing download logic. This separation also enabled independent evolution of both hierarchies—new downloader types and new source implementations can be added independently.

The *Decorator pattern* provided exceptional flexibility for building processing pipelines. Instead of creating separate classes for every possible combination of operations (FormatConvertingWatermarkingResizer, FormatConvertingResizer, etc.), decorators allow dynamic composition at runtime. Users can apply operations in any order, and the system remains open for extension (new decorators) while closed for modification. The pattern's transparency—decorators implementing the same interface as the component they wrap—enabled seamless chaining.

The *Proxy pattern* significantly optimized system performance through intelligent caching. By storing base MP4 files identified by URL hash, the proxy eliminates redundant network requests. The careful design ensures caching happens before decorator transformations, allowing the same cached content to be processed with different operations. This separation of concerns—proxy handles access control and caching, decorators handle transformations—demonstrates how structural patterns complement each other.

Overall, the integration of these patterns helped me showcase their synergy, as the Bridge provides the foundation, Proxy adds optimization, and Decorator enables flexibility. Each pattern solves a specific structural problem while maintaining loose coupling and high cohesion. The resulting system is extensible, through new sources and new decorators, efficient, via caching, and maintainable, clear separation of concerns.

---

## References

1. Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1994). *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley Professional.

2. Refactoring Guru. *Structural Design Patterns*. Retrieved from https://refactoring.guru/design-patterns/structural-patterns

3. Refactoring Guru. *Decorator Pattern*. Retrieved from https://refactoring.guru/design-patterns/decorator

4. Refactoring Guru. *Bridge Pattern*. Retrieved from https://refactoring.guru/design-patterns/bridge

5. Refactoring Guru. *Proxy Pattern*. Retrieved from https://refactoring.guru/design-patterns/proxy
