using System;
using System.Collections.Generic;
using System.Linq;

public static class Lab05
{
    public static void Main()
    {
        var items = new (int w, int v, int t)[]
        {
            (3, 6, 4), (4, 7, 5), (5, 10, 6),
            (2, 4, 3), (6, 12, 7)
        };

        Console.WriteLine($"Эвакуация: {Evacuation(items, 10, 12)}");

        int[] keys = { 10, 20, 30, 40 };
        int[] frequencies = { 3, 2, 4, 1 };
        Console.WriteLine($"Оптимальное дерево поиска: {OptimalBst(keys, frequencies)}");

        var orders = new (int d, int p)[]
        {
            (6, 11), (8, 14), (5, 9), (7, 13), (4, 6)
        };

        var cutting = Cutting(20, orders, 2);
        Console.WriteLine(
            $"Раскрой: прибыль {cutting.profit}; " +
            $"заказы {string.Join(", ", cutting.orders)}");
    }

    public static int Evacuation(
        (int w, int v, int t)[] items,
        int maxWeight,
        int maxTime)
    {
        int n = items.Length;
        int[,,] dp = new int[n + 1, maxWeight + 1, maxTime + 1];

        for (int i = 1; i <= n; i++)
        {
            var item = items[i - 1];

            for (int w = 0; w <= maxWeight; w++)
            for (int t = 0; t <= maxTime; t++)
            {
                dp[i, w, t] = dp[i - 1, w, t];

                if (item.w <= w && item.t <= t)
                {
                    int value = dp[
                        i - 1,
                        w - item.w,
                        t - item.t] + item.v;

                    dp[i, w, t] =
                        Math.Max(dp[i, w, t], value);
                }
            }
        }

        return dp[n, maxWeight, maxTime];
    }

    public static int OptimalBst(int[] keys, int[] freq)
    {
        int n = keys.Length;
        int[,] cost = new int[n, n];
        int[] prefix = new int[n + 1];

        for (int i = 0; i < n; i++)
            prefix[i + 1] = prefix[i] + freq[i];

        for (int length = 1; length <= n; length++)
        for (int left = 0; left + length <= n; left++)
        {
            int right = left + length - 1;
            cost[left, right] = int.MaxValue;
            int sum = prefix[right + 1] - prefix[left];

            for (int root = left; root <= right; root++)
            {
                int leftCost =
                    root > left ? cost[left, root - 1] : 0;
                int rightCost =
                    root < right ? cost[root + 1, right] : 0;

                int current = leftCost + rightCost + sum;
                cost[left, right] =
                    Math.Min(cost[left, right], current);
            }
        }

        return cost[0, n - 1];
    }

    public static (int profit, List<int> orders) Cutting(
        int length,
        (int d, int p)[] orders,
        int cutCost)
    {
        int[] dp = new int[length + 1];
        List<int>[] selected = new List<int>[length + 1];

        for (int i = 0; i <= length; i++)
            selected[i] = new List<int>();

        for (int i = 0; i < orders.Length; i++)
        {
            int size = orders[i].d;
            int gain = orders[i].p - cutCost;

            for (int used = length; used >= size; used--)
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

        return (dp[length], selected[length]);
    }
}

