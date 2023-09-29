using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class PriceRepository : EFBaseRepository<Price, ApplicationDbContext>, IPriceRepository
{
    public PriceRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Price>> AllAsync()
    {
        return await RepositoryDbSet
            .Include(p => p.Product)
            .ToListAsync();
    }

    public override async Task<Price?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .Include(p => p.Product)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}