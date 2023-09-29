using System.Text.Json.Serialization;
using App.Domain.Identity;
using Domain.Base;

namespace App.Domain;

public class Cart : DomainEntityId
{
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceIncludingVat { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public ICollection<CartProduct>? CartProducts { get; set; }

}