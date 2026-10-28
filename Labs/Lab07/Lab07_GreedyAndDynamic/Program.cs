using System;
using System.Collections.Generic;
using System.Linq;

public static class Lab7
{
    public static void Main()
    {
        var exhibits = new (int weight, int price)[]
        {
            (4, 10), (7, 16), (3, 8), (5, 14), (2, 4)
        };

        var stolen = MuseumGreedy(exhibits, visits: 2, capacity: 7);
        Console.WriteLine($"Жадный алгоритм: экспонаты {string.Join(", ", stolen)}");
        Console.WriteLine($"Стоимость: {stolen.Sum(i => exhibits[i - 1].price)} руб.");

        var values = new[] { -2, 1, 3, 2, 4, 5, 6, 0, 7, 8 };
        var run = LongestIncreasingRun(values);
        Console.WriteLine($"Возрастaющая серия: {string.Join(", ", values.Skip(run.start).Take(run.length))}");
        Console.WriteLine($"Длина серии: {run.length}");

        var items = new (int weight, int value, int time)[]
        {
            (3, 6, 4), (4, 7, 5), (5, 10, 6), (2, 4, 3), (6, 12, 7)
        };
        Console.WriteLine($"Эвакуация: {Evacuation(items, 10, 12)}");

        var orders = new (int length, int price)[]
        {
            (6, 11), (8, 14), (5, 9), (7, 13), (4, 6)
        };
        var cutting = Cutting(20, orders, 2);
        Console.WriteLine($"Раскрой: прибыль {cutting.profit} руб.; заказы {string.Join(", ", cutting.selected)}");
    }

    // Жадный алгоритм: на каждом шаге выбирается доступный предмет
    // с наибольшим отношением цены к весу.
    public static List<int> MuseumGreedy(
        (int weight, int price)[] items,
        int visits,
        int capacity)
    {
        var order = items
            .Select((item, index) => new
            {
                Index = index + 1,
                item.weight,
                item.price,
                Ratio = (double)item.price / item.weight
            })
            .OrderByDescending(x => x.Ratio)
            .ToList();

        var result = new List<int>();
        var used = new bool[items.Length];

        for (int visit = 0; visit < visits; visit++)
        {
            int remaining = capacity;

            foreach (var item in order)
            {
                if (used[item.Index - 1] || item.weight > remaining)
                    continue;

                used[item.Index - 1] = true;
                result.Add(item.Index);
                remaining -= item.weight;
            }
        }

        return result;
    }

    // Поиск максимальной непрерывной строго возрастающей серии.
    public static (int start, int length) LongestIncreasingRun(int[] values)
    {
        if (values.Length == 0)
            return (-1, 0);

        int bestStart = 0;
        int bestLength = 1;
        int currentStart = 0;
        int currentLength = 1;

        for (int i = 1; i < values.Length; i++)
        {
            if (values[i] > values[i - 1])
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

    // Динамическое программирование с ограничениями по весу и времени.
    public static int Evacuation(
        (int weight, int value, int time)[] items,
        int maxWeight,
        int maxTime)
    {
        int[,,] dp = new int[items.Length + 1, maxWeight + 1, maxTime + 1];

        for (int i = 1; i <= items.Length; i++)
        {
            var item = items[i - 1];

            for (int weight = 0; weight <= maxWeight; weight++)
            for (int time = 0; time <= maxTime; time++)
            {
                dp[i, weight, time] = dp[i - 1, weight, time];

                if (item.weight <= weight && item.time <= time)
                {
                    int candidate = dp[
                        i - 1,
                        weight - item.weight,
                        time - item.time] + item.value;

                    dp[i, weight, time] =
                        Math.Max(dp[i, weight, time], candidate);
                }
            }
        }

        return dp[items.Length, maxWeight, maxTime];
    }

    // Динамическое программирование для раскроя.
    public static (int profit, List<int> selected) Cutting(
        int materialLength,
        (int length, int price)[] orders,
        int cutCost)
    {
        int[] dp = new int[materialLength + 1];
        var selected = new List<int>[materialLength + 1];

        for (int i = 0; i <= materialLength; i++)
            selected[i] = new List<int>();

        for (int i = 0; i < orders.Length; i++)
        {
            int size = orders[i].length;
            int gain = orders[i].price - cutCost;

            for (int used = materialLength; used >= size; used--)
            {
                int candidate = dp[used - size] + gain;

                if (candidate > dp[used])
                {
                    dp[used] = candidate;
                    selected[used] =
                        new List<int>(selected[used - size]);
                    selected[used].Add(i + 1);
                }
            }
        }

        return (dp[materialLength], selected[materialLength]);
    }
}

