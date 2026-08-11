namespace learning_entity_framework.Models;

public class Enrollment
{
    public Guid StudentId { get; set; }
    public virtual Student? Student { get; set; }
    public Guid CouseId { get; set; }
    public virtual Course? Course { get; set; }
    public DateTime EnrollmentAt { get; set; }
}