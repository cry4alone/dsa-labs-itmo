namespace Lab02_Task2_10Functions
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
    }

    public class Letter
    {
        public string Sender { get; set; }
        public DateTime Date { get; set; }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class Musician
    {
        public string Name { get; set; }
        public string Instrument { get; set; }
    }

    public class Program
    {
        // 1. O(2n) - Прочитать список покупок дважды: проверить наличие и посчитать сумму
        public void ProcessShoppingList(List<Product> shoppingList)
        {
            foreach (var p in shoppingList)
            {
                if (!p.InStock) Console.WriteLine($"  {p.Name} отсутствует");
            }

            decimal total = 0;
            foreach (var p in shoppingList)
            {
                if (p.InStock) total += p.Price;
            }
            Console.WriteLine($"  Итого: {total}");
        }

        // 2. O(2n) - Два прохода по списку товаров: найти максимум и минимум
        public void FindMinMaxPrices(List<Product> products)
        {
            decimal max = decimal.MinValue;
            foreach (var p in products)
            {
                if (p.Price > max) max = p.Price;
            }

            decimal min = decimal.MaxValue;
            foreach (var p in products)
            {
                if (p.Price < min) min = p.Price;
            }

            Console.WriteLine($"  Мин: {min}, Макс: {max}");
        }

        // 3. O(n log n) - Отсортировать стопку писем по дате
        public void SortLettersByDate(List<Letter> letters)
        {
            letters.Sort((a, b) => a.Date.CompareTo(b.Date));
            foreach (var letter in letters)
            {
                Console.WriteLine($"  {letter.Sender} - {letter.Date:dd.MM.yyyy}");
            }
        }

        // 4. O(n log n) - Отсортировать письма по имени отправителя
        public void SortLettersBySender(List<Letter> letters)
        {
            letters.Sort((a, b) => string.Compare(a.Sender, b.Sender, StringComparison.Ordinal));
            foreach (var letter in letters)
            {
                Console.WriteLine($"  {letter.Sender} - {letter.Date:dd.MM.yyyy}");
            }
        }

        // 5. O(2n³) - Перебрать все тройки ингредиентов для двух разных блюд
        public void GenerateDishCombinations(List<Ingredient> ingredients)
        {
            Console.WriteLine("  Блюдо 1:");
            for (int i = 0; i < ingredients.Count; i++)
                for (int j = i + 1; j < ingredients.Count; j++)
                    for (int k = j + 1; k < ingredients.Count; k++)
                        Console.WriteLine($"    {ingredients[i].Name} + {ingredients[j].Name} + {ingredients[k].Name}");

            Console.WriteLine("  Блюдо 2:");
            for (int i = 0; i < ingredients.Count; i++)
                for (int j = i + 1; j < ingredients.Count; j++)
                    for (int k = j + 1; k < ingredients.Count; k++)
                        Console.WriteLine($"    {ingredients[i].Name} + {ingredients[j].Name} + {ingredients[k].Name}");
        }

        // 6. O(2n³) - Проверить все тройки товаров на скидку для двух акций
        public void CheckTripletsForPromotions(List<Product> products)
        {
            Console.WriteLine("  Акция 1 (сумма > 1000):");
            for (int i = 0; i < products.Count; i++)
                for (int j = i + 1; j < products.Count; j++)
                    for (int k = j + 1; k < products.Count; k++)
                    {
                        decimal sum = products[i].Price + products[j].Price + products[k].Price;
                        if (sum > 1000) Console.WriteLine($"    {products[i].Name}, {products[j].Name}, {products[k].Name} = {sum}");
                    }

            Console.WriteLine("  Акция 2 (сумма < 500):");
            for (int i = 0; i < products.Count; i++)
                for (int j = i + 1; j < products.Count; j++)
                    for (int k = j + 1; k < products.Count; k++)
                    {
                        decimal sum = products[i].Price + products[j].Price + products[k].Price;
                        if (sum < 500) Console.WriteLine($"    {products[i].Name}, {products[j].Name}, {products[k].Name} = {sum}");
                    }
        }

        // 7. O(n!) - Составить все возможные порядки выступления музыкантов
        public void GeneratePerformanceOrders(List<Musician> musicians, int left, int right)
        {
            if (left == right)
            {
                Console.WriteLine("  " + string.Join(" -> ", musicians.Select(m => m.Name)));
            }
            else
            {
                for (int i = left; i <= right; i++)
                {
                    Swap(musicians, left, i);
                    GeneratePerformanceOrders(musicians, left + 1, right);
                    Swap(musicians, left, i);
                }
            }
        }

        // 8. O(n!) - Составить все возможные порядки рассадки музыкантов в автобусе
        public void GenerateBusSeating(List<Musician> musicians, int left, int right)
        {
            if (left == right)
            {
                Console.WriteLine("  " + string.Join(", ", musicians.Select(m => m.Name)));
            }
            else
            {
                for (int i = left; i <= right; i++)
                {
                    Swap(musicians, left, i);
                    GenerateBusSeating(musicians, left + 1, right);
                    Swap(musicians, left, i);
                }
            }
        }

        // 9. O(2 log n) - Угадать два загаданных числа бинарным поиском
        public void GuessTwoNumbers(int[] sortedRange, int target1, int target2)
        {
            Console.WriteLine($"  Поиск числа {target1}:");
            BinarySearch(sortedRange, target1);
            Console.WriteLine($"  Поиск числа {target2}:");
            BinarySearch(sortedRange, target2);
        }

        // 10. O(2 log n) - Найти точки вставки для двух новых чисел
        public void FindInsertionPoints(int[] sortedRange, int value1, int value2)
        {
            Console.WriteLine($"  Поиск позиции для {value1}:");
            BinarySearchInsertion(sortedRange, value1);
            Console.WriteLine($"  Поиск позиции для {value2}:");
            BinarySearchInsertion(sortedRange, value2);
        }

        private void Swap<T>(List<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        private void BinarySearch(int[] arr, int target)
        {
            int left = 0, right = arr.Length - 1;
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (arr[mid] == target) { Console.WriteLine($"    Найдено: {target}"); return; }
                if (arr[mid] < target) left = mid + 1;
                else right = mid - 1;
            }
            Console.WriteLine($"    {target} не найдено");
        }

        private void BinarySearchInsertion(int[] arr, int value)
        {
            int left = 0, right = arr.Length;
            while (left < right)
            {
                int mid = left + (right - left) / 2;
                if (arr[mid] < value) left = mid + 1;
                else right = mid;
            }
            Console.WriteLine($"    Позиция вставки для {value}: {left}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторная работа №2 - Сложность алгоритмов ===\n");
            
            var program = new Program();

            // Тестовые данные
            var shoppingList = new List<Product>
            {
                new Product { Name = "Хлеб", Price = 50, InStock = true },
                new Product { Name = "Молоко", Price = 80, InStock = false },
                new Product { Name = "Сыр", Price = 200, InStock = true },
                new Product { Name = "Колбаса", Price = 350, InStock = true }
            };

            var letters = new List<Letter>
            {
                new Letter { Sender = "Иванов", Date = new DateTime(2026, 9, 20) },
                new Letter { Sender = "Петров", Date = new DateTime(2026, 9, 15) },
                new Letter { Sender = "Сидоров", Date = new DateTime(2026, 9, 24) },
                new Letter { Sender = "Алексеев", Date = new DateTime(2026, 9, 18) }
            };

            var ingredients = new List<Ingredient>
            {
                new Ingredient { Name = "Мука", Category = "Основа" },
                new Ingredient { Name = "Сахар", Category = "Добавка" },
                new Ingredient { Name = "Яйца", Category = "Основа" },
                new Ingredient { Name = "Масло", Category = "Добавка" }
            };

            var musicians = new List<Musician>
            {
                new Musician { Name = "Андрей", Instrument = "Гитара" },
                new Musician { Name = "Борис", Instrument = "Барабаны" },
                new Musician { Name = "Виктор", Instrument = "Бас" }
            };

            int[] sortedRange = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            // Функция 1: O(2n)
            Console.WriteLine("1. O(2n) - Обработка списка покупок:");
            program.ProcessShoppingList(shoppingList);
            Console.WriteLine();

            // Функция 2: O(2n)
            Console.WriteLine("2. O(2n) - Поиск мин/макс цен:");
            program.FindMinMaxPrices(shoppingList);
            Console.WriteLine();

            // Функция 3: O(n log n)
            Console.WriteLine("3. O(n log n) - Сортировка писем по дате:");
            var lettersCopy1 = new List<Letter>(letters);
            program.SortLettersByDate(lettersCopy1);
            Console.WriteLine();

            // Функция 4: O(n log n)
            Console.WriteLine("4. O(n log n) - Сортировка писем по отправителю:");
            var lettersCopy2 = new List<Letter>(letters);
            program.SortLettersBySender(lettersCopy2);
            Console.WriteLine();

            // Функция 5: O(2n³)
            Console.WriteLine("5. O(2n³) - Комбинации ингредиентов для двух блюд:");
            program.GenerateDishCombinations(ingredients);
            Console.WriteLine();

            // Функция 6: O(2n³)
            Console.WriteLine("6. O(2n³) - Проверка троек товаров для акций:");
            program.CheckTripletsForPromotions(shoppingList);
            Console.WriteLine();

            // Функция 7: O(n!)
            Console.WriteLine("7. O(n!) - Порядки выступления музыкантов:");
            var musiciansCopy1 = new List<Musician>(musicians);
            program.GeneratePerformanceOrders(musiciansCopy1, 0, musiciansCopy1.Count - 1);
            Console.WriteLine();

            // Функция 8: O(n!)
            Console.WriteLine("8. O(n!) - Рассадка музыкантов в автобусе:");
            var musiciansCopy2 = new List<Musician>(musicians);
            program.GenerateBusSeating(musiciansCopy2, 0, musiciansCopy2.Count - 1);
            Console.WriteLine();

            // Функция 9: O(2 log n)
            Console.WriteLine("9. O(2 log n) - Поиск двух чисел:");
            program.GuessTwoNumbers(sortedRange, 40, 75);
            Console.WriteLine();

            // Функция 10: O(2 log n)
            Console.WriteLine("10. O(2 log n) - Поиск точек вставки:");
            program.FindInsertionPoints(sortedRange, 35, 85);
            Console.WriteLine();

            Console.WriteLine("=== Завершено ===");
        }
    }
}