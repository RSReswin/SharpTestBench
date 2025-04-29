using System.Net;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace SharpTestBench.Benches;

public static class MediaTookKitBench
{
    private static readonly Uri VideoUrl = new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4");
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
}