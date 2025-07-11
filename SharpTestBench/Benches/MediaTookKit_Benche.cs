using System.Net;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Xabe.FFmpeg;

namespace SharpTestBench.Benches;

public static class MediaTookKitBench
{
    private static readonly Uri VideoUrl =
        new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4");

    private const string FfmpegPath = @"D:\Others\DevTools\Ffmpeg\ffmpeg.exe";

    public static async Task VideoUrlToSingeImageConverterAsync()
    {
        try
        {
            var webClient = new WebClient();
            byte[] videoBuffer = webClient.DownloadData(VideoUrl);
            webClient.Dispose();

            string tempVideoPath = Path.Combine(Path.GetTempPath(), "temp_video.mp4");
            await File.WriteAllBytesAsync(tempVideoPath, videoBuffer);

            var inputFile = new MediaFile { Filename = tempVideoPath };
            var outputFile = new MediaFile { Filename = "temp_image.jpg" };

            var engine = new Engine(FfmpegPath);
            engine.GetMetadata(inputFile);
            engine.GetThumbnail(inputFile, outputFile, new ConversionOptions { Seek = TimeSpan.FromSeconds(6) });
            engine.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static async Task ConvertVideoToM3U8FormateAsync()
    {
        FFmpeg.SetExecutablesPath(@"D:\Others\DevTools\Ffmpeg");
    
        string inputPath = @"C:\Users\Administrator\Desktop\video\input\sample.mp4";
        string outputRoot = @"C:\Users\Administrator\Desktop\video\output";
        string thumbnailOutputRoot = @"C:\Users\Administrator\Desktop\video\output\thumbnail.jpg";
        
        Func<string, string> outputFileNameBuilder = (number) => "fileNameNo" + number + ".png";
        
        var variants = new[]
        {
            new { Folder = "720p", Resolution = "1280x720", VideoBitrate = new Mbps(1), AudioBitrate = new Kbps(128) },
            new { Folder = "480p", Resolution = "854x480",  VideoBitrate = new Mbps(0.8),  AudioBitrate = new Kbps(96) },
            new { Folder = "360p", Resolution = "640x360",   VideoBitrate = new Mbps(0.5),  AudioBitrate = new Kbps(64) }
        };
        
        var thumbConversion = FFmpeg.Conversions.New()
            .AddParameter($"-ss 00:00:30", ParameterPosition.PreInput)
            .AddParameter($"-i \"{inputPath}\"", ParameterPosition.PreInput)
            .AddParameter("-vframes 1")
            .AddParameter($"\"{thumbnailOutputRoot}\"")
            .SetOverwriteOutput(true);
        
        await thumbConversion.Start();
        Console.WriteLine("🖼️ Thumbnail extracted");

        for (int i = 0; i < variants.Length; i++)
        {
            string outDir = Path.Combine(outputRoot, variants[i].Folder);
            Directory.CreateDirectory(outDir);
            string output = Path.Combine(outDir, "index.m3u8");
    
            var conversion = FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\"", ParameterPosition.PreInput)
                .AddParameter($"-vf scale={variants[i].Resolution}")
                .AddParameter($"-c:v libx264 -crf 30 -preset veryslow -profile:v baseline")
                .AddParameter($"-c:a aac -ar 48000 -b:a {(int)variants[i].AudioBitrate}")
                .AddParameter("-g 48 -keyint_min 48")
                .AddParameter("-sc_threshold 0")
                .AddParameter("-hls_time 4 -hls_playlist_type vod")
                .AddParameter($"-b:v {(int)variants[i].VideoBitrate}")
                .AddParameter($"-maxrate {(int)variants[i].VideoBitrate} -bufsize {(int)variants[i].VideoBitrate * 2}")
                .AddParameter("-f hls")
                .AddParameter($"\"{output}\"")
                .SetFrameRate(20)
                .UseMultiThread(true)
                .SetOverwriteOutput(true);
            
            // conversion.OnDataReceived += (sender, args) => Console.WriteLine(args.Data);

            var i1 = i;
            conversion.OnProgress += (sender, args) =>
            {
                var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.Write($"\r⏳ [{i1 + 1}/{variants.Length}] Progress: {percent}%    ");
            };
            
            await conversion.Start();
        }
    
        // 📝 Generate master.m3u8 playlist
        string masterPath = Path.Combine(outputRoot, "master.m3u8");
        await using var writer = new StreamWriter(masterPath);
        await writer.WriteLineAsync("#EXTM3U");
    
        foreach (var v in variants)
        {
            int totalBandwidth = (int)v.VideoBitrate + (int)v.AudioBitrate;
            await writer.WriteLineAsync($"#EXT-X-STREAM-INF:BANDWIDTH={totalBandwidth},RESOLUTION={v.Resolution}");
            await writer.WriteLineAsync($"{v.Folder}/index.m3u8");
        }
        
        Console.WriteLine("✅ Compressed adaptive HLS streaming output complete.");
    }

    public readonly struct Mbps(double value)
    {
        private double Value => value;

        public static implicit operator int(Mbps x) => (int)(x.Value * 1_000_000);
    }
    
    public readonly struct Kbps(double value)
    {
        private double Value => value;

        public static implicit operator int(Kbps x) => (int)(x.Value * 1_000);
    }
}