namespace SharpTestBench.Benches;

public static class IdeHotReload_Benche
{
    public static async Task StartLogAsync()
    {
        while (true)
        {
            await Task.Delay(1000);
            Console.WriteLine("it is working fine!");
        }
    }
}