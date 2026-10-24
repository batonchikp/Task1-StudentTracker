using System.Globalization;
using StudentTracker;

Console.OutputEncoding = System.Text.Encoding.UTF8;

StudentManager manager = new StudentManager();
bool running = true;

while (running)
{
    ShowMenu();
    string choice = Read("Ваш выбор");

    switch (choice)
    {
        case "1": AddStudent(); break;
        case "2": ShowAll(); break;
        case "3": SearchByName(); break;
        case "4": SearchById(); break;
        case "5": RemoveStudent(); break;
        case "6": SortStudents(); break;
        case "7": ShowStats(); break;
        case "0": running = false; continue;
        default: Line("Такого пункта нет. Введите число из меню."); break;
    }

    Pause();
}

Line("Работа завершена. До встречи!");

void ShowMenu()
{
    Line("");
    Line("======== УЧЁТ СТУДЕНТОВ ========");
    Line(" 1. Добавить студента");
    Line(" 2. Показать всех");
    Line(" 3. Найти по фамилии");
    Line(" 4. Найти по ID");
    Line(" 5. Удалить по ID");
    Line(" 6. Отсортировать список");
    Line(" 7. Статистика");
    Line(" 0. Выход");
}

void AddStudent()
{
    Line("");
    Line("--- Новый студент ---");
    string fullName = ReadNonEmpty("ФИО");
    string group = ReadNonEmpty("Группа (например БИС-1-19)");
    double grade = ReadGrade("Средний балл (от 0 до 5)");

    Student student = new Student(fullName, group, grade);
    manager.Add(student);
    Line($"Готово: студент добавлен под номером {student.Id}.");
}

void ShowAll()
{
    if (manager.Count == 0)
    {
        Line("");
        Line("Список пока пуст. Добавьте студента — пункт 1.");
        return;
    }

    PrintTable(manager.SortedByName());
}

void SearchByName()
{
    Line("");
    string part = ReadNonEmpty("Введите фамилию или её часть");
    List<Student> found = manager.FindByName(part);

    if (found.Count == 0)
    {
        Line($"Никого с «{part}» в списке нет.");
        return;
    }

    PrintTable(found);
}

void SearchById()
{
    Line("");
    int id = ReadId("Введите ID студента");
    Student? student = manager.FindById(id);

    if (student is null)
    {
        Line($"Студента с ID {id} нет в списке.");
        return;
    }

    PrintTable(new List<Student> { student });
}

void RemoveStudent()
{
    Line("");
    int id = ReadId("Введите ID студента для удаления");
    Student? student = manager.FindById(id);

    if (student is null)
    {
        Line($"Студента с ID {id} нет — удалять нечего.");
        return;
    }

    Line($"Будет удалён: {student.FullName} ({student.Group})");
    string answer = Read("Точно удалить? (да/нет)");

    if (answer.Equals("да", StringComparison.OrdinalIgnoreCase) || answer.Equals("y", StringComparison.OrdinalIgnoreCase))
    {
        manager.Remove(id);
        Line("Удалено.");
    }
    else
    {
        Line("Отмена: студент остался в списке.");
    }
}

void SortStudents()
{
    if (manager.Count == 0)
    {
        Line("");
        Line("Список пуст — сортировать нечего.");
        return;
    }

    Line("");
    Line("Как сортировать? 1 — по ФИО, 2 — по среднему баллу");
    string choice = Read("Ваш выбор");

    switch (choice)
    {
        case "1": PrintTable(manager.SortedByName()); break;
        case "2": PrintTable(manager.SortedByGrade()); break;
        default: Line("Отмена."); break;
    }
}

void ShowStats()
{
    Line("");
    if (manager.Count == 0)
    {
        Line("Список пуст — считать нечего.");
        return;
    }

    Line("--- Статистика ---");
    Line($"Всего студентов: {manager.Count}");
    Line($"Средний балл по всем: {manager.AverageGrade():F2}");

    Student? best = manager.Best();
    if (best is not null)
    {
        Line($"Лучший по баллу: {best.FullName} ({best.AverageGrade:F2})");
    }

    Line("");
    Line("По группам:");
    foreach ((string group, int count, double average) in manager.GroupStats())
    {
        Line($"  {group,-12} студентов: {count,-3} средний балл: {average:F2}");
    }
}

void PrintTable(List<Student> list)
{
    Line("");
    Line(new string('-', 56));
    Line($"{"ID",4}  {"ФИО",-28}  {"Группа",-10}  {"Балл",6}");
    Line(new string('-', 56));
    foreach (Student student in list)
    {
        Line(student.ToTableRow());
    }
    Line(new string('-', 56));
    Line($"Записей: {list.Count}");
}

string Read(string prompt)
{
    Console.Write($"{prompt}: ");
    return (Console.ReadLine() ?? string.Empty).Trim();
}

string ReadNonEmpty(string prompt)
{
    while (true)
    {
        string value = Read(prompt);
        if (value.Length > 0)
        {
            return value;
        }

        Line("Пусто. Введите значение.");
    }
}

double ReadGrade(string prompt)
{
    while (true)
    {
        string value = Read(prompt).Replace(',', '.');
        bool parsed = double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double grade);

        if (parsed && grade >= 0 && grade <= 5)
        {
            return grade;
        }

        Line("Нужно число от 0 до 5, например 4.75 или 4,75");
    }
}

int ReadId(string prompt)
{
    while (true)
    {
        string value = Read(prompt);
        if (int.TryParse(value, out int id))
        {
            return id;
        }

        Line("Нужно целое число, например 3");
    }
}

void Line(string text) => Console.WriteLine(text);

void Pause()
{
    Console.WriteLine();
    Read("Нажмите Enter, чтобы вернуться в меню");
}
