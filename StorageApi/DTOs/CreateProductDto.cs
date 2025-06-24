using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs;

public class CreateProductDto
{
    [Required]
    [MaxLength(80)]
    public string Name { get; set; }

    [Range(1, 1000000, ErrorMessage = "Price must be greater than 0.")]
    public int Price { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; }

    [Required]
    [MaxLength(50)]
    public string Shelf { get; set; }

    [Range(0, 10000, ErrorMessage = "Count cannot be negative.")]
    public int Count { get; set; }

    [Required]
    [MaxLength(250)]
    public string Description { get; set; }
}
