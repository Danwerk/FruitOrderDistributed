using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICartProductService: IBaseRepository<App.BLL.DTO.CartProduct>, ICartProductRepositoryCustom<App.BLL.DTO.CartProduct>
{
    // add your custom service methods here
}