using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IDiscountService : IBaseRepository<App.BLL.DTO.Discount>,
    IDiscountRepositoryCustom<App.BLL.DTO.Discount>
{
    Task<App.BLL.DTO.Discount?> FirstOrDefaultAsync(Guid id);

    Task<App.BLL.DTO.Discount?> GetActiveDiscountAsync(Guid productId);

    // add your custom service methods here
}