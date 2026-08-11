using System.ComponentModel.DataAnnotations;

namespace learning_entity_framework.Models;

public class PersonAddress
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    public Guid PersonId {get; set;}
    public virtual Person? Person { get; set; } // navigation property
}