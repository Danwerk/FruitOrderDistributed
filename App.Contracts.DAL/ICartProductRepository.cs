using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ICartProductRepository : IBaseRepository<CartProduct>,  ICartProductRepositoryCustom<CartProduct>
{
    
}

public interface ICartProductRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
    public Task<IEnumerable<TEntity>> AllAsync(Guid userId);
   
}

