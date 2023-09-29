using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Domain.Base;

namespace App.BLL.DTO;

public class Order : DomainEntityId
{

    [MaxLength(256)]
    public string OrderNr { get; set; } = default!;

    public DateTime OrderDate { get; set; }

    [MaxLength(128)]
    public string Status { get; set; } = default!;

    public Guid? PaymentId { get; set; }
    
    // public Payment? Payment { get; set; }

    [MaxLength(256)]
    public string OrdererName { get; set; } = default!;
    
    [MaxLength(256)]
    public string OrdererEmail { get; set; } = default!;
    
    [MaxLength(256)]
    public string OrdererAddress { get; set; } = default!;
    
    [MaxLength(256)]
    public string OrdererPhoneNumber { get; set; } = default!;
    
    public DateTime OrderDeliveryTime { get; set; }

    public decimal TotalPriceWithoutVat { get; set; }
    public decimal TotalPriceIncludingVat { get; set; }
    public decimal DeliveryPrice { get; set; }
    public decimal Total { get; set; }
    public Guid AppUserId { get; set; }
    
    public AppUser? AppUser { get; set; }

    public ICollection<OrderProduct>? OrderProducts { get; set; }
}