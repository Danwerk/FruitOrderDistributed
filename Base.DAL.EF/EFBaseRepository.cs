using Base.Contracts.DAL;
using Domain.Contracts.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class EFBaseRepository<TEntity, TDbContext> : EFBaseRepository<TEntity, Guid, TDbContext>,
    IBaseRepository<TEntity>
    where TEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    public EFBaseRepository(TDbContext dataContext) : base(dataContext)
    {
    }
}

public class EFBaseRepository<TEntity, TKey, TDbContext> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    protected TDbContext RepositoryDbContext;
    protected DbSet<TEntity> RepositoryDbSet;

    public EFBaseRepository(TDbContext dataContext)
    {
        RepositoryDbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        RepositoryDbSet = RepositoryDbContext.Set<TEntity>();
    }


    public virtual async Task<IEnumerable<TEntity>> AllAsync()
    {
        return await RepositoryDbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> FindAsync(TKey id)
    {
        return await RepositoryDbSet.FindAsync(id);
    }

    public virtual TEntity Add(TEntity entity)
    {
        return RepositoryDbSet.Add(entity).Entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        return RepositoryDbSet.Update(entity).Entity;
    }

    public virtual TEntity Remove(TEntity entity)
    {
        return RepositoryDbSet.Remove(entity).Entity;
    }


    public virtual async Task<TEntity?> RemoveAsync(TKey id)
    {
        var entity = await FindAsync(id);
        return entity != null ? Remove(entity) : null;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(TKey id)
    {
        return await RepositoryDbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }
}