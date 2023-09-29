using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICartService: IBaseRepository<App.BLL.DTO.Cart>, ICartRepositoryCustom<App.BLL.DTO.Cart>
{
    // add your custom service methods here
}