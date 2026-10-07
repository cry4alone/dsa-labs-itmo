using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace Lab04_Task02_SubstringSearch;

internal static class Program
{
    private const int TextLength = 500;
    private const int Modulus = 1_000_000_007;
    private const int Base = 257;

    private static string BuildFibonacciText()
    {
        var text = new StringBuilder();
        BigInteger previous = 0;
        BigInteger current = 1;

        for (var i = 0; i < TextLength; i++)
        {
            text.Append((i == 0 ? previous : current).ToString());
            (previous, current) = (current, previous + current);
        }

        return text.ToString();
    }

    private static int NaiveCount(string text, string pattern)
    {
        var count = 0;
        for (var start = 0; start <= text.Length - pattern.Length; start++)
        {
            var matched = true;
            for (var offset = 0; offset < pattern.Length; offset++)
            {
                if (text[start + offset] == pattern[offset]) continue;
                matched = false;
                break;
            }

            if (matched) count++;
        }

        return count;
    }

    private static int RabinKarpCount(string text, string pattern)
    {
        if (pattern.Length > text.Length) return 0;

        var patternHash = Hash(pattern);
        var windowHash = Hash(text[..pattern.Length]);
        var highestPower = 1;
        for (var i = 1; i < pattern.Length; i++)
            highestPower = (int)((long)highestPower * Base % Modulus);

        var count = 0;
        for (var start = 0; start <= text.Length - pattern.Length; start++)
        {
            if (windowHash == patternHash && text.AsSpan(start, pattern.Length).SequenceEqual(pattern))
                count++;

            if (start == text.Length - pattern.Length) continue;
            windowHash = (int)(((windowHash - (long)text[start] * highestPower % Modulus + Modulus)
                * Base + text[start + pattern.Length]) % Modulus);
        }

        return count;
    }

    private static int BoyerMooreCount(string text, string pattern)
    {
        if (pattern.Length > text.Length) return 0;

        var shifts = new int[char.MaxValue + 1];
        Array.Fill(shifts, pattern.Length);
        for (var i = 0; i < pattern.Length - 1; i++)
            shifts[pattern[i]] = pattern.Length - 1 - i;

        var count = 0;
        var start = 0;
        while (start <= text.Length - pattern.Length)
        {
            var offset = pattern.Length - 1;
            while (offset >= 0 && pattern[offset] == text[start + offset]) offset--;
            if (offset < 0)
            {
                count++;
                start++;
            }
            else
            {
                start += shifts[text[start + pattern.Length - 1]];
            }
        }

        return count;
    }

    private static int KnuthMorrisPrattCount(string text, string pattern)
    {
        var prefix = BuildPrefixTable(pattern);
        var count = 0;
        var matched = 0;

        foreach (var t in text)
        {
            while (matched > 0 && t != pattern[matched])
                matched = prefix[matched - 1];
            if (t == pattern[matched]) matched++;

            if (matched != pattern.Length) continue;
            count++;
            matched = prefix[matched - 1];
        }

        return count;
    }

    private static int[] BuildPrefixTable(string pattern)
    {
        var prefix = new int[pattern.Length];
        var length = 0;
        for (var i = 1; i < pattern.Length; i++)
        {
            while (length > 0 && pattern[i] != pattern[length])
                length = prefix[length - 1];
            if (pattern[i] == pattern[length]) length++;
            prefix[i] = length;
        }

        return prefix;
    }

    private static int Hash(string value)
    {
        var hash = 0;
        foreach (var character in value)
            hash = (int)(((long)hash * Base + character) % Modulus);
        return hash;
    }

    private static void Main()
    {
        var text = BuildFibonacciText();
        var algorithms = new (string Name, Func<string, string, int> Search)[]
        {
            ("Наивный поиск", NaiveCount),
            ("Рабин-Карп", RabinKarpCount),
            ("Бойер-Мур", BoyerMooreCount),
            ("Кнут-Моррис-Пратт", KnuthMorrisPrattCount)
        };

        Console.WriteLine("=== Лабораторная работа №4. Задание 2 ===");
        Console.WriteLine($"Строка содержит {text.Length} символов.");
        Console.WriteLine("Используются числа Фибоначчи, так как вариант нечётный.");
        Console.WriteLine("\nНаиболее часто встречающиеся двузначные числа:");

        Dictionary<string, int>? expectedCounts = null;
        foreach (var pair in algorithms)
        {
            var stopwatch = Stopwatch.StartNew();
            var counts = new Dictionary<string, int>();
            for (var value = 10; value <= 99; value++)
            {
                var pattern = value.ToString();
                counts[pattern] = pair.Search(text, pattern);
            }
            stopwatch.Stop();

            if (expectedCounts is not null &&
                counts.Any(item => expectedCounts[item.Key] != item.Value))
                throw new InvalidOperationException(
                    $"{pair.Name} вернул результаты, отличающиеся от эталона.");
            expectedCounts ??= counts;

            var maximum = counts.Values.Max();
            var mostFrequent = counts
                .Where(item => item.Value == maximum)
                .Select(item => item.Key);
            Console.WriteLine($"{pair.Name}: {string.Join(", ", mostFrequent)} — {maximum} раз(а), {stopwatch.ElapsedTicks} ticks");
        }
    }
}
