using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IOrderProductService: IBaseRepository<App.BLL.DTO.OrderProduct>,
    IOrderProductRepositoryCustom<App.BLL.DTO.OrderProduct>
{
    // add your custom service methods here
}