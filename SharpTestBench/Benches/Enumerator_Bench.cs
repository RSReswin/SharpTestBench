using System.Collections;

namespace SharpTestBench.Benches;

public static class Enumerator_Bench
{
    public static void IterateFromCollection()
    {
        var list = new List<int>(){1,2,3,4,5};
        var items = list.GetEnumerator();

        while (items.MoveNext())
        {
            Console.WriteLine(items.Current);
        }
        items.Dispose();
    }
}