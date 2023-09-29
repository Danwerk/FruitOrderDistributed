using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IOrderService : IBaseRepository<App.BLL.DTO.Order>, IOrderRepositoryCustom<App.BLL.DTO.Order>
{
    // add your custom service methods here
}