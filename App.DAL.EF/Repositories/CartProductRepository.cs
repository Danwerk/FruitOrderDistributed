using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class CartProductRepository : EFBaseRepository<CartProduct, ApplicationDbContext>, ICartProductRepository
{
    public CartProductRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<CartProduct>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(c => c.Cart)
            .Include(c => c.Product).OrderBy(c => c.ProductId).ToListAsync();
    }
    
    public virtual async Task<IEnumerable<CartProduct>> AllAsync(Guid userId)
    {
        return await RepositoryDbSet
            .Include(e => e.Cart)
            .Include(e => e.Product)
            .Where(e => e.Cart!.AppUserId == userId).OrderBy(c => c.ProductId)
            .ToListAsync();
    }

    
    public override async Task<CartProduct?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(c => c.Cart)
            .Include(c => c.Product)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}    