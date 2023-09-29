using Domain.Base;

namespace App.Domain;

public class Price : DomainEntityId
{
    public decimal Value { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}