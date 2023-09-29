using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IOrderProductRepository : IBaseRepository<OrderProduct>, IOrderProductRepositoryCustom<OrderProduct>
{
}

public interface IOrderProductRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
    public Task<IEnumerable<TEntity>> AllAsync(Guid userId);
}