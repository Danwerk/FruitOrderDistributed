using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IOrderRepository : IBaseRepository<Order>, IOrderRepositoryCustom<Order>
{
    // add here custom methods for repo only 
}

public interface IOrderRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
    public Task<IEnumerable<TEntity>> AllAsync(Guid userId);
    
    public Task<TEntity?> FindAsync(Guid id, Guid userId);
}