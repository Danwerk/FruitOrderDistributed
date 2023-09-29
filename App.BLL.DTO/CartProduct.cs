using Domain.Base;

namespace App.BLL.DTO;

public class CartProduct : DomainEntityId
{
    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Total { get; set; }

    public int Discount { get; set; }

    public Guid ProductId { get; set; }
    // public Product? Product { get; set; }

    public Guid CartId { get; set; }
    // public Cart? Cart { get; set; }
}