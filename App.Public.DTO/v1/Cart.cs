namespace App.Public.DTO.v1;

public class Cart
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceIncludingVat { get; set; }
    public Guid AppUserId { get; set; }

    public ICollection<CartProduct>? CartProducts { get; set; }
    
    
}