using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IProductRepository : IBaseRepository<Product>,  IProductRepositoryCustom<Product>
{
    
}

public interface IProductRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
   
}
