using App.Domain;
using App.Domain.Identity;
using DAL.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Product = App.Public.DTO.v1.Product;

namespace Tests.WebApp;

public class CustomWebAppFactory<TStartup> : WebApplicationFactory<TStartup> 
    where TStartup : class
{
    private static bool dbInitialized = false;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureServices(services =>
        {
            // find DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));

            // if found - remove
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // and new DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // create db and seed data
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<CustomWebAppFactory<TStartup>>>();

            db.Database.EnsureCreated();
            
            
             try
            {
                if (dbInitialized == false)
                {
                    dbInitialized = true;
                    // DataSeeder.SeedData(db);
                
                    var appUser = new AppUser()
                    {
                        Id = Guid.NewGuid(),
                        Email = "some@account.com",
                        PhoneNumber = "0123456789",
                        FirstName = "some",
                        LastName = "account",
                        UserName = "some",
                        EmailConfirmed = true
                    };
                    db.Users.Add(appUser);
                    db.SaveChanges();
                    
                    var unit = new Unit()
                    {
                        Id = Guid.Parse("727b6f6d-6ddd-4820-8306-4628b5609893"),
                        UnitName = "Test unit",
                    };
                    db.Units.Add(unit);
                    db.SaveChanges();
            
                    var productType = new ProductType()
                    {
                        Id = Guid.Parse("0f6368ec-f708-48ea-ae64-be5ca7fbad75"),
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        Name = "Mr productType",
                    };
                    db.ProductTypes.Add(productType);
                    db.SaveChanges();
                    
                
                    if (db.Carts.Any()) return;
                    var cart = new Cart()
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        AppUserId = appUser.Id,
                        TotalPrice = 12345,
                        
                        };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    "database with test messages. Error: {Message}", ex.Message);
            }

        

            
            
            
            
        });

    }
}