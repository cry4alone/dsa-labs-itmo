namespace Lab03_Task01_Sorting;

internal static class Program
{
    private static void QuickSort(int[] array, int left, int right)
    {
        if (left >= right) return;
        var pivot = array[left + (right - left) / 2];
        var i = left;
        var j = right;
        while (i <= j)
        {
            while (array[i] < pivot) i++;
            while (array[j] > pivot) j--;
            if (i <= j)
            {
                (array[i], array[j]) = (array[j], array[i]);
                i++;
                j--;
            }
        }
        if (left < j) QuickSort(array, left, j);
        if (i < right) QuickSort(array, i, right);
    }

    private static void MergeSort(int[] array)
    {
        if (array.Length < 2) return;
        var buffer = new int[array.Length];
        MergeSort(array, buffer, 0, array.Length - 1);
    }

    private static void MergeSort(int[] array, int[] buffer, int left, int right)
    {
        if (left >= right) return;
        var middle = left + (right - left) / 2;
        MergeSort(array, buffer, left, middle);
        MergeSort(array, buffer, middle + 1, right);
        var i = left;
        var j = middle + 1;
        var k = left;
        while (i <= middle && j <= right)
            buffer[k++] = array[i] <= array[j] ? array[i++] : array[j++];
        while (i <= middle) buffer[k++] = array[i++];
        while (j <= right) buffer[k++] = array[j++];
        for (var index = left; index <= right; index++) array[index] = buffer[index];
    }

    private static void CombSort(int[] array)
    {
        var gap = array.Length;
        var swapped = true;
        while (gap > 1 || swapped)
        {
            gap = Math.Max(1, (int)(gap / 1.3));
            swapped = false;
            for (var i = 0; i + gap < array.Length; i++)
            {
                if (array[i] <= array[i + gap]) continue;
                (array[i], array[i + gap]) = (array[i + gap], array[i]);
                swapped = true;
            }
        }
    }

    private static void Main()
    {
        var source = new[] { 42, -7, 15, 0, 23, 15, 8, -20, 4 };
        var algorithms = new (string Name, Action<int[]> Sort)[]
        {
            ("Быстрая сортировка", values => QuickSort(values, 0, values.Length - 1)),
            ("Сортировка слиянием", MergeSort),
            ("Сортировка расчёской", CombSort)
        };

        Console.WriteLine("=== Лабораторная работа №3. Задание 1 ===");
        foreach (var algorithm in algorithms)
        {
            var values = (int[])source.Clone();
            algorithm.Sort(values);
            Console.WriteLine($"{algorithm.Name}: {string.Join(", ", values)}");
        }
    }
}
