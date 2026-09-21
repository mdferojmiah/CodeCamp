using System.ComponentModel.DataAnnotations;

namespace learning_validation_mediatr.Dtos;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Product name is requiered.")]
    [StringLength(10, ErrorMessage = "Product name can not exceeds 10 characters.")]
    public string Name { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue, ErrorMessage = "Price can't be zero")]
    public decimal Price { get; set; }
}