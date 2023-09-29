using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class OrderProductRepository : EFBaseRepository<OrderProduct, ApplicationDbContext>, IOrderProductRepository
{
    public OrderProductRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<OrderProduct>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(o => o.Order)
            .Include(o => o.Product)
            .ToListAsync();
    }
    
    public virtual async Task<IEnumerable<OrderProduct>> AllAsync(Guid userId)
    {
        return await RepositoryDbSet
            .Include(e => e.Order)
            .Include(e => e.Product)
            .Where(e => e.Order!.AppUserId == userId)
            .ToListAsync();
    }

    public override async Task<OrderProduct?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(o => o.Order)
            .Include(o => o.Product)
            .FirstOrDefaultAsync(m => m.Id == id);;
    }
}