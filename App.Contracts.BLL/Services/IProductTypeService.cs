using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IProductTypeService : IBaseRepository<App.BLL.DTO.ProductType>, IProductTypeRepositoryCustom<App.BLL.DTO.ProductType>
{
    // add your custom service methods here
}