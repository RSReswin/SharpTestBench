namespace SharpTestBench.Benches;

public static class Span_Benche
{
    private const string FileName = "image.jpg";

    public static void SliceFileName()
    {
        ReadOnlySpan<char> timedName = FileName.AsSpan().Slice(0, FileName.Length - 4);
        Console.WriteLine(timedName.ToString());
    }

    public static void TrimFileName()
    {
        ReadOnlySpan<char> timedName = FileName.AsSpan().TrimEnd(".jpg");
        Console.WriteLine(timedName.ToString());
    }
}