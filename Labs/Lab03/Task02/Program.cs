namespace Lab03_Task02_Sorting;

internal static class Program
{
    private static void BucketSort(int[] array)
    {
        if (array.Length < 2) return;
        var minimum = array.Min();
        var maximum = array.Max();
        if (minimum == maximum) return;

        var buckets = new List<int>[Math.Max(1, (int)Math.Sqrt(array.Length))];
        for (var i = 0; i < buckets.Length; i++) buckets[i] = [];
        var range = (long)maximum - minimum + 1;
        foreach (var value in array)
        {
            var index = (int)(((long)value - minimum) * buckets.Length / range);
            buckets[Math.Min(index, buckets.Length - 1)].Add(value);
        }

        var position = 0;
        foreach (var bucket in buckets)
        {
            bucket.Sort();
            foreach (var value in bucket) array[position++] = value;
        }
    }

    private static void HeapSort(int[] array)
    {
        for (var i = array.Length / 2 - 1; i >= 0; i--) SiftDown(array, i, array.Length);
        for (var end = array.Length - 1; end > 0; end--)
        {
            (array[0], array[end]) = (array[end], array[0]);
            SiftDown(array, 0, end);
        }
    }

    private static void SiftDown(int[] array, int root, int length)
    {
        while (true)
        {
            var child = root * 2 + 1;
            if (child >= length) return;
            if (child + 1 < length && array[child] < array[child + 1]) child++;
            if (array[root] >= array[child]) return;
            (array[root], array[child]) = (array[child], array[root]);
            root = child;
        }
    }

    private static void Main()
    {
        var source = new[] { 42, -7, 15, 0, 23, 15, 8, -20, 4 };
        var algorithms = new (string Name, Action<int[]> Sort)[]
        {
            ("Блочная сортировка", BucketSort),
            ("Пирамидальная сортировка", HeapSort)
        };

        Console.WriteLine("=== Лабораторная работа №3. Задание 2 ===");
        foreach (var algorithm in algorithms)
        {
            var values = (int[])source.Clone();
            algorithm.Sort(values);
            Console.WriteLine($"{algorithm.Name}: {string.Join(", ", values)}");
        }
    }
}
