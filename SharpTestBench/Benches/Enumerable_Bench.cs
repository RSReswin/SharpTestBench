namespace SharpTestBench.Benches;

public static class Enumerable_Bench
{
    public static void EnumerableWithCollection()
    {
       IEnumerable<int> items = [1,2,3,4,5];

        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    public static void EnumerableWithMethodCollection()
    {
        var items = Numbers();

        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    private static IEnumerable<int> Numbers()
    {
        yield return 1;
        yield return 2;
        yield return 3;
        yield return 4;
        yield return 5;
    }
}