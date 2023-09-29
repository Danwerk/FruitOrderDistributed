namespace App.Public.DTO.v1;

public class Price
{
    public Guid Id { get; set; }
    
    public decimal Value { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public Guid ProductId { get; set; }
    // public Product? Product { get; set; }
    
}