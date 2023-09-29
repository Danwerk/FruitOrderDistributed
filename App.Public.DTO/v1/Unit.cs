using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class Unit
{

    public Guid Id { get; set; }
    [MaxLength(128)]
    public string UnitName { get; set; } = default!;

    public ICollection<Product>? Products { get; set; }
}