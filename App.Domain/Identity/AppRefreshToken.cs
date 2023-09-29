using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace App.Domain.Identity;

public class AppRefreshToken : BaseRefreshToken
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}