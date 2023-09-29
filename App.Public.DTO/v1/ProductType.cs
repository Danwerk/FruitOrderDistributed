using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class ProductType
{
    public Guid Id { get; set; }
    
    [MaxLength(512)]
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Product>? Products { get; set; }
}