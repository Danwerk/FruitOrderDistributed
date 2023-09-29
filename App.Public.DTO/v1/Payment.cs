using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class Payment
{
    public Guid Id { get; set; }
    
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