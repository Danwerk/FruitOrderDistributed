using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IProductService: IBaseRepository<App.BLL.DTO.Product>, IProductRepositoryCustom<App.BLL.DTO.Product>
{
    // add your custom service methods here
    Task<IEnumerable<App.BLL.DTO.Product>> AllAsync();

    Task<App.BLL.DTO.Product?> FirstOrDefaultAsync(Guid id);
    
}