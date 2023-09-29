using System.ComponentModel.DataAnnotations;
using Domain.Contracts.Base;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser :  IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(1)]
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;

    [MinLength(1)]
    [MaxLength(128)]
    public string LastName { get; set; } = default!;

    public ICollection<AppRefreshToken>? AppRefreshTokens { get; set; }

    public ICollection<Cart>? Carts { get; set; }
    public ICollection<Order>? Orders { get; set; }
    
}