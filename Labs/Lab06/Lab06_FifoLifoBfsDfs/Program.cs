using System;
using System.Collections.Generic;
using System.Linq;

public static class Lab06
{
    public static void Main()
    {
        string[] bracketExamples =
        {
            "()", "(())()", ")( ", "([)]", "([{}])", "((()"
        };

        foreach (string text in bracketExamples)
        {
            var result = CheckBrackets(text.Trim());
            Console.WriteLine(
                $"{text}: " +
                $"{(result.correct ? "правильная" : "ошибка " + result.errorPosition)}");
        }

        var graph = new Dictionary<string, List<string>>
        {
            ["A"] = new() { "B", "C" },
            ["B"] = new() { "A", "D" },
            ["C"] = new() { "A", "D", "E" },
            ["D"] = new() { "B", "C", "F" },
            ["E"] = new() { "C", "F" },
            ["F"] = new() { "D", "E" }
        };

        Console.WriteLine(
            $"BFS: {string.Join(" -> ", Bfs(graph, "A", "E"))}");

        var dfsPath = new List<string>();
        Dfs(graph, "A", "E", new HashSet<string>(), dfsPath);
        Console.WriteLine($"DFS: {string.Join(" -> ", dfsPath)}");
    }

    static (bool correct, int errorPosition) CheckRoundBrackets(
        string text)
    {
        Stack<int> stack = new();

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '(')
                stack.Push(i + 1);
            else if (text[i] == ')')
            {
                if (stack.Count == 0)
                    return (false, i + 1);

                stack.Pop();
            }
        }

        if (stack.Count > 0)
            return (false, stack.Min());

        return (true, -1);
    }

    static bool Matches(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}');
    }

    static (bool correct, int errorPosition) CheckBrackets(
        string text)
    {
        Stack<(char bracket, int position)> stack = new();

        for (int i = 0; i < text.Length; i++)
        {
            char symbol = text[i];

            if (symbol is '(' or '[' or '{')
                stack.Push((symbol, i + 1));
            else if (symbol is ')' or ']' or '}')
            {
                if (stack.Count == 0)
                    return (false, i + 1);

                var top = stack.Pop();

                if (!Matches(top.bracket, symbol))
                    return (false, i + 1);
            }
        }

        return stack.Count == 0
            ? (true, -1)
            : (false, stack.Peek().position);
    }

    static List<string> Bfs(
        Dictionary<string, List<string>> graph,
        string start,
        string target)
    {
        Queue<string> queue = new();
        Dictionary<string, string?> parent = new();
        HashSet<string> visited = new();

        queue.Enqueue(start);
        visited.Add(start);
        parent[start] = null;

        while (queue.Count > 0)
        {
            string current = queue.Dequeue();

            if (current == target)
                break;

            foreach (string next in graph[current])
            {
                if (visited.Contains(next))
                    continue;

                visited.Add(next);
                parent[next] = current;
                queue.Enqueue(next);
            }
        }

        return RestorePath(parent, start, target);
    }

    static bool Dfs(
        Dictionary<string, List<string>> graph,
        string current,
        string target,
        HashSet<string> visited,
        List<string> path)
    {
        visited.Add(current);
        path.Add(current);

        if (current == target)
            return true;

        foreach (string next in graph[current])
        {
            if (!visited.Contains(next) &&
                Dfs(graph, next, target, visited, path))
                return true;
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }

    static List<string> RestorePath(
        Dictionary<string, string?> parent,
        string start,
        string target)
    {
        var path = new List<string>();

        if (!parent.ContainsKey(target))
            return path;

        string? current = target;

        while (current != null)
        {
            path.Add(current);
            current = parent[current];
        }

        path.Reverse();
        return path;
    }
}

