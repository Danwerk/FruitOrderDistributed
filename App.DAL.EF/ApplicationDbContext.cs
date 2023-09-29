using App.Domain;
using App.Domain.Identity;
using Domain.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<CartProduct> CartProducts { get; set; } = default!;
    public DbSet<Discount> Discounts { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderProduct> OrderProducts { get; set; } = default!;
    public DbSet<Payment> Payments { get; set; } = default!;
    public DbSet<Price> Prices { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductType> ProductTypes { get; set; } = default!;
    public DbSet<Unit> Units { get; set; } = default!;
    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // disable cascade delete
        foreach (var foreignKey in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
       
    }
}