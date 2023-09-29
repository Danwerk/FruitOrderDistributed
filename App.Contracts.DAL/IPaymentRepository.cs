using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IPaymentRepository : IBaseRepository<Payment>, IPaymentRepositoryCustom<Payment>
{
}

public interface IPaymentRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
}