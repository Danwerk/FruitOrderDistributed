using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.Domain;

public class Unit : DomainEntityId
{
    [MaxLength(128)]
    public string UnitName { get; set; } = default!;

    public ICollection<Product>? Products { get; set; }
}