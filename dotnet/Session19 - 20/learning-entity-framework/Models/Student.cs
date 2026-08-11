namespace learning_entity_framework.Models;

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Enrollment> Enrollments { get; set; } = new();
}