using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace learning_entity_framework.Models;

[Table("Users")]
public class Person
{
    // [Key]
    public Guid Id { get; set; }

    // [Required]
    // [MaxLength(100)]
    // [MinLength(3)]
    public string  Name { get; set; } = string.Empty;

    // [Column("User_Email")]
    // [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // [NotMapped]
    public string Dispalyname => $"{Name} {Email}";

    public virtual PersonAddress? PersonAddress { get; set; }
    public virtual List<Order>? Orders { get; set; }
}