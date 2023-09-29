using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPaymentService: IBaseRepository<App.BLL.DTO.Payment>,
    IPaymentRepositoryCustom<App.BLL.DTO.Payment>
{
    // add your custom service methods here
}