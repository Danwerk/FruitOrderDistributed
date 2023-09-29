using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IDiscountRepository : IBaseRepository<Discount>, IDiscountRepositoryCustom<Discount>
{
}

public interface IDiscountRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
}