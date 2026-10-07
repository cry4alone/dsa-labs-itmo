namespace Lab04_Task01_IncreasingSequence;

internal static class Program
{
    private static (int Start, int Length) FindLongestIncreasingSequence(int[] numbers)
    {
        if (numbers.Length == 0) return (-1, 0);

        var bestStart = 0;
        var bestLength = 1;
        var currentStart = 0;
        var currentLength = 1;

        for (var i = 1; i < numbers.Length; i++)
        {
            if (numbers[i - 1] < numbers[i])
            {
                currentLength++;
            }
            else
            {
                currentStart = i;
                currentLength = 1;
            }

            if (currentLength > bestLength)
            {
                bestStart = currentStart;
                bestLength = currentLength;
            }
        }

        return (bestStart, bestLength);
    }

    private static void Main()
    {
        var random = new Random(2026);
        var numbers = Enumerable.Range(0, 20)
            .Select(_ => random.Next(-100, 101))
            .ToArray();
        var result = FindLongestIncreasingSequence(numbers);
        var sequence = numbers.Skip(result.Start).Take(result.Length);

        Console.WriteLine($"Массив: {string.Join(", ", numbers)}");
        Console.WriteLine($"Длина максимальной возрастающей серии: {result.Length}");
        Console.WriteLine($"Серия: {string.Join(", ", sequence)}");
        Console.WriteLine($"Индексы: {result.Start}..{result.Start + result.Length - 1}");
    }
}
