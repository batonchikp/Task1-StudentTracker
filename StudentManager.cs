namespace StudentTracker;

public class StudentManager
{
    private readonly List<Student> students = new();

    public int Count => students.Count;

    public void Add(Student student) => students.Add(student);

    public Student? FindById(int id) => students.FirstOrDefault(s => s.Id == id);

    public List<Student> FindByName(string part) =>
        students.Where(s => s.FullName.Contains(part, StringComparison.OrdinalIgnoreCase)).ToList();

    public bool Remove(int id)
    {
        Student? student = FindById(id);
        return student is not null && students.Remove(student);
    }

    public List<Student> SortedByName() => students.OrderBy(s => s.FullName).ToList();

    public List<Student> SortedByGrade() => students.OrderByDescending(s => s.AverageGrade).ToList();

    public double AverageGrade() => students.Count == 0 ? 0 : students.Average(s => s.AverageGrade);

    public Student? Best() => students.MaxBy(s => s.AverageGrade);

    public List<(string Group, int Count, double Average)> GroupStats() =>
        students.GroupBy(s => s.Group)
                .Select(g => (Group: g.Key, Count: g.Count(), Average: g.Average(s => s.AverageGrade)))
                .OrderBy(x => x.Group)
                .ToList();
}
