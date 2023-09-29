using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EFBaseUOW<TDbContext> : IBaseUOW
where TDbContext: DbContext
{
    protected readonly TDbContext UowDbContext;
    public EFBaseUOW(TDbContext dataContext)
    {
        UowDbContext = dataContext;
    }
    
    public async virtual Task<int> SaveChangesAsync()
    {
        return await UowDbContext.SaveChangesAsync();
    }
    
     
}