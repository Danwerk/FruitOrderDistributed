using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class ProductTypeRepository : EFBaseRepository<ProductType, ApplicationDbContext>, IProductTypeRepository
{
    public ProductTypeRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<IEnumerable<ProductType>> AllAsync()
    {
        return await RepositoryDbSet.ToListAsync();
    }

    public override async Task<ProductType?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}