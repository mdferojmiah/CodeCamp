namespace learning_entity_framework.Models;

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Enrollment> Enrollments { get; set; } = new();
}