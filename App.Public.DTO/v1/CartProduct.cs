namespace App.Public.DTO.v1;

public class CartProduct
{
    public Guid Id { get; set; }
    
    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Total { get; set; }

    public int Discount { get; set; }
    
    public Guid ProductId { get; set; }

    public Guid CartId { get; set; }
}