using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class Discount
{
    public Guid Id { get; set; }
    
    public int DiscountValue { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    [MaxLength(4096)]
    public string Description { get; set; } = default!;

    public Guid ProductId { get; set; }
    // public Product? Product { get; set; }
}