namespace StudentTracker;

public class Student
{
    private static int lastId = 0;

    public int Id { get; }
    public string FullName { get; set; }
    public string Group { get; set; }
    public double AverageGrade { get; set; }

    public Student(string fullName, string group, double averageGrade)
    {
        Id = ++lastId;
        FullName = fullName;
        Group = group;
        AverageGrade = averageGrade;
    }

    public string ToTableRow() => $"{Id,4}  {FullName,-28}  {Group,-10}  {AverageGrade,6:F2}";
}
