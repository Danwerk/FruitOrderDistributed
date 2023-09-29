using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class OrderRepository : EFBaseRepository<Order, ApplicationDbContext>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Order>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(e => e.AppUser)
            .Include(o => o.Payment)
            .Include(o => o.OrderProducts)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<Order>> AllAsync(Guid userId)
    {
        return await RepositoryDbSet
            .Include(e => e.AppUser)
            .Include(o => o.Payment)
            .Include(o =>o.OrderProducts)
            .Where(c => c.AppUserId == userId)
            .ToListAsync();
    }
    

    public override async Task<Order?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(c => c.AppUser)
            .Include(o => o.Payment)
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public virtual async Task<Order?> FindAsync(Guid id, Guid userId)
    {
        return await RepositoryDbSet
            .Include(c => c.AppUser)
            .Include(o => o.Payment)
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(m => m.Id == id && m.AppUserId == userId);
    }
}