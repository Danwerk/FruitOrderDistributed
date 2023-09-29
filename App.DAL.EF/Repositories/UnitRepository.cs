using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class UnitRepository : EFBaseRepository<Unit, ApplicationDbContext>, IUnitRepository
{
    public UnitRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<Unit>> AllAsync()
    {
        return await RepositoryDbSet.ToListAsync();
    }

    public override async Task<Unit?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}