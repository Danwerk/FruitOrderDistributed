using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class DiscountRepository : EFBaseRepository<Discount, ApplicationDbContext>, IDiscountRepository
{
    public DiscountRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Discount>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(d => d.Product)
            .ToListAsync();
    }

    public override async Task<Discount?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(d => d.Product)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

   
}