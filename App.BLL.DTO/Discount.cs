using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.BLL.DTO;

public class Discount : DomainEntityId
{
    public int DiscountValue { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    
    [MaxLength(4096)]
    public string Description { get; set; } = default!;

    public Guid ProductId { get; set; }
    // public Product? Product { get; set; }
}
