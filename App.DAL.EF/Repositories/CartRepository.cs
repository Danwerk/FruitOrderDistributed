using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class CartRepository : EFBaseRepository<Cart, ApplicationDbContext>, ICartRepository
{
    public CartRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Cart>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(c=>c.CartProducts)
            .Include(e => e.AppUser)
            .ToListAsync();
    }
    
    public virtual async Task<IEnumerable<Cart>> AllAsync(Guid userId)
    {
        return await RepositoryDbSet
            .Include(e => e.AppUser)
            .Include(c => c.CartProducts)
            .Where(c => c.AppUserId == userId)
            .ToListAsync();
    }

    public override async Task<Cart?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(c => c.AppUser)
            .Include(c => c.CartProducts)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    
    public virtual async Task<Cart?> FindAsync(Guid id, Guid userId)
    {
        return await RepositoryDbSet
            .Include(c => c.AppUser)
            .Include(c => c.CartProducts)
            .FirstOrDefaultAsync(m => m.Id == id && m.AppUserId == userId);
    }


}