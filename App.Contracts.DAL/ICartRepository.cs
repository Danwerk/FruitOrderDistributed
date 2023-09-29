using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface ICartRepository : IBaseRepository<Cart>,  ICartRepositoryCustom<Cart>
{

}

public interface ICartRepositoryCustom<TEntity>
{
    //add here shared methods between repo and service
    public Task<IEnumerable<TEntity>> AllAsync(Guid userId);

    public Task<TEntity?> FindAsync(Guid id, Guid userId);
}