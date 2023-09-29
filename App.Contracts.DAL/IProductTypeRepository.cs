using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IProductTypeRepository : IBaseRepository<ProductType>, IProductTypeRepositoryCustom<ProductType>
{
}

public interface IProductTypeRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
}