using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPriceService : IBaseRepository<App.BLL.DTO.Price>,
    IPriceRepositoryCustom<App.BLL.DTO.Price>
{
    // add your custom service methods here
    
    Task<App.BLL.DTO.Price?> GetActivePriceAsync(Guid productId);
}