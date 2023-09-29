using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProductRepository : EFBaseRepository<Product, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Product>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(p => p.ProductType)
            .Include(p => p.Unit)
            .Include(p=>p.Discounts)
            .Include(p=> p.CartProducts)
            .Include(p => p.Prices)
            .ToListAsync();
    }

    public override async Task<Product?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(p => p.ProductType)
            .Include(p => p.Unit)
            .Include(p => p.Discounts)
            .Include(p =>p.CartProducts)
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}