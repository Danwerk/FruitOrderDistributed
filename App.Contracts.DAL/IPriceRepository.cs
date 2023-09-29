using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPriceRepository : IBaseRepository<Price>, IPriceRepositoryCustom<Price>
{
}

public interface IPriceRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
}