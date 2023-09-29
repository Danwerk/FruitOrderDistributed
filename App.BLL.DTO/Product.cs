using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.BLL.DTO;

public class Product : DomainEntityId
{
    public string Image { get; set; } = default!;

    [MaxLength(512)] public string Name { get; set; } = default!;

    [MaxLength(4096)] public string Description { get; set; } = default!;

    public int Quantity { get; set; }

    public Guid UnitId { get; set; }
    // public Unit? Unit { get; set; }

    public Guid ProductTypeId { get; set; }
    // public ProductType? ProductType { get; set; }

    public ICollection<Discount>? Discounts { get; set; }
    public ICollection<Price>? Prices { get; set; }
    public ICollection<CartProduct>? CartProducts { get; set; }
    public ICollection<OrderProduct>? OrderProducts { get; set; }
}
