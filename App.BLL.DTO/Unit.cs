using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.BLL.DTO;

public class Unit : DomainEntityId
{
    [MaxLength(128)]
    public string UnitName { get; set; } = default!;

    public ICollection<Product>? Products { get; set; }
}