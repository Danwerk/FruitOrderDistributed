using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IUnitRepository : IBaseRepository<Unit>,  IUnitRepositoryCustom<Unit>
{
    
}

public interface IUnitRepositoryCustom<TEntity>
{

    //add here shared methods between repo and service
   
}