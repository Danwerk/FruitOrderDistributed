using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.BLL.DTO;

public class Payment : DomainEntityId
{
    [MaxLength(256)]
    public string Name { get; set; } = default!;

    [MaxLength(128)]
    public string CardNo { get; set; } = default!;

    public DateTime ExpiryDate { get; set; }

    [MaxLength(32)]
    public string CvvNo { get; set; } = default!;

    [MaxLength(512)]
    public string Address { get; set; } = default!;

    [MaxLength(512)]
    public string PaymentMode { get; set; } = default!;

    public ICollection<Order>? Orders { get; set; }
}