namespace Lab01_Task02_StudentGuesser;

public record Student(
    string FullName,
    bool IsSmoking,
    bool IsBrunette,
    bool IsItmoBachelor,
    bool IsSport,
    bool IsPets,
    string InterestingFact);

public sealed record Question(string Text, Func<Student, bool> Predicate);

public static class Group
{
    public static readonly IReadOnlyList<Student> Students =
    [
        new Student("Егорова Мария Викторовна",
            IsSmoking: false, IsBrunette: true, IsItmoBachelor: false,
            IsSport: true, IsPets: true,
            InterestingFact: "Закончила художественную школу"),

        new Student("Фролова Кристина Ивановна",
            IsSmoking: false, IsBrunette: false, IsItmoBachelor: true,
            IsSport: true, IsPets: false,
            InterestingFact: "У меня ночевал голубь"),

        new Student("Костюченко Тимофей Романович",
            IsSmoking: false, IsBrunette: true, IsItmoBachelor: false,
            IsSport: true, IsPets: false,
            InterestingFact: "Есть дорожный знак с наклейками"),

        new Student("Портнова Ксения",
            IsSmoking: true, IsBrunette: false, IsItmoBachelor: true,
            IsSport: false, IsPets: true,
            InterestingFact: "Играю на барабанах"),

        new Student("Кенжаев Рахим Ермахмадович",
            IsSmoking: false, IsBrunette: true, IsItmoBachelor: true,
            IsSport: true, IsPets: false,
            InterestingFact: "ДНДшник"),

        new Student("Даньшин Семён",
            IsSmoking: true, IsBrunette: true, IsItmoBachelor: true,
            IsSport: true, IsPets: false,
            InterestingFact: "Семён лимон"),

        new Student("Шихайло Сергей Владимирович",
            IsSmoking: true, IsBrunette: false, IsItmoBachelor: true,
            IsSport: true, IsPets: true,
            InterestingFact: "Текущий профиль работы — 3D-визуализация"),

        new Student("Бабушкин Александр Михайлович",
            IsSmoking: true, IsBrunette: false, IsItmoBachelor: true,
            IsSport: false, IsPets: false,
            InterestingFact: "Я сделал покер"),

        new Student("Якунин Андрей Денисович",
            IsSmoking: false, IsBrunette: false, IsItmoBachelor: true,
            IsSport: true, IsPets: false,
            InterestingFact: "Гойда"),

        new Student("Казанцев Александр",
            IsSmoking: false, IsBrunette: false, IsItmoBachelor: false,
            IsSport: true, IsPets: false,
            InterestingFact: "Автостоп по Европе"),

        new Student("Юркин Александр",
            IsSmoking: true, IsBrunette: true, IsItmoBachelor: true,
            IsSport: true, IsPets: false,
            InterestingFact: "Фанат МЮ"),

        new Student("Панкратова Анна Алексеевна",
            IsSmoking: true, IsBrunette: false, IsItmoBachelor: false,
            IsSport: true, IsPets: true,
            InterestingFact: "На выпускном из бакалавриата каталась на тазах, а окружающие обливали всех пивом"),

        new Student("Колесникова Лариса Эдуардовна",
            IsSmoking: false, IsBrunette: false, IsItmoBachelor: false,
            IsSport: true, IsPets: false,
            InterestingFact: "Люблю вайбовые фотографии, сделанные на мыльницу"),
    ];
}

public static class QuestionCatalog
{
    public static readonly IReadOnlyList<Question> All =
    [
        new Question("Студент курит?",                        s => s.IsSmoking),
        new Question("Студент брюнет?",                       s => s.IsBrunette),
        new Question("Студент учился в ИТМО в бакалавриате?", s => s.IsItmoBachelor),
        new Question("Студент занимается спортом?",           s => s.IsSport),
        new Question("У студента есть домашние животные?",    s => s.IsPets),
    ];
}

public sealed class Guesser(IEnumerable<Student> students, IReadOnlyList<Question> questions)
{
    private readonly HashSet<string> _askedTexts = [];
    private List<Student> _candidates = students.ToList();

    public IReadOnlyList<Student> Candidates => _candidates;

    public bool IsFinished => _candidates.Count <= 1;

    public Question? ChooseNextQuestion()
    {
        Question? best = null;
        var bestScore = int.MaxValue;

        foreach (var q in questions)
        {
            if (_askedTexts.Contains(q.Text)) continue;

            var yes = _candidates.Count(q.Predicate);
            var no = _candidates.Count - yes;
            var score = Math.Max(yes, no);

            if (score >= bestScore) continue;

            bestScore = score;
            best = q;
        }

        return best;
    }

    public void ApplyAnswer(Question question, bool answer)
    {
        _candidates = _candidates
            .Where(s => question.Predicate(s) == answer)
            .ToList();
        _askedTexts.Add(question.Text);
    }
}

internal static class Program
{
    public static void Main()
    {
        var guesser = new Guesser(Group.Students, QuestionCatalog.All);

        Console.WriteLine("Загадайте любого студента из группы и отвечайте «Да» или «Нет».");

        while (!guesser.IsFinished)
        {
            var question = guesser.ChooseNextQuestion();
            if (question is null) break;

            Console.Write($">> {question.Text} (Да/Нет): ");
            var answer = ReadYesNo();
            guesser.ApplyAnswer(question, answer);
        }

        PrintResult(guesser.Candidates);
    }

    private static bool ReadYesNo()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim().ToLower();
            switch (input)
            {
                case "да" or "yes" or "y" or "д":
                    return true;
                case "нет" or "no" or "n" or "н":
                    return false;
                default:
                    Console.Write("Пожалуйста, ответьте «Да» или «Нет»: ");
                    break;
            }
        }
    }

    private static void PrintResult(IReadOnlyList<Student> candidates)
    {
        if (candidates.Count == 1)
        {
            Console.WriteLine($">> Вы загадали {candidates[0].FullName}.");
            return;
        }

        Console.WriteLine(">> Не смог однозначно угадать. Возможные варианты:");
        foreach (var s in candidates)
            Console.WriteLine($"   - {s.FullName} ({s.InterestingFact})");
    }
}