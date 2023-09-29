using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.BLL.DTO;

public class ProductType : DomainEntityId
{
    [MaxLength(512)]
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Product>? Products { get; set; }
}