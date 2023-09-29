namespace Base.Contracts.DAL;
using Domain.Contracts.Base;


public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

public interface IBaseRepository<TEntity, in TKey>
    where TEntity : class , IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    // IEnumerable<TEntity> All();
    Task<IEnumerable<TEntity>> AllAsync();

    // TEntity Find(TKey id);
    Task<TEntity?> FindAsync(TKey id);

    TEntity Add(TEntity entity);
    
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    
    Task<TEntity?> RemoveAsync(TKey id);
   Task<TEntity?> FirstOrDefaultAsync(TKey id);
    
    
    
}