using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DiscountService:
    BaseEntityService<App.BLL.DTO.Discount, App.Domain.Discount, IDiscountRepository>, IDiscountService
{
    protected IAppUOW Uow;

    public DiscountService(IAppUOW uow, IMapper<App.BLL.DTO.Discount, App.Domain.Discount> mapper)
        : base(uow.DiscountRepository, mapper)
    {
        Uow = uow;
    }

    
    public async Task<Discount?> GetActiveDiscountAsync(Guid productId)
    {
        // todo: solve possible null problem for product.discounts
        var product = await Uow.ProductRepository.FindAsync(productId);
        var activeDiscount = product!.Discounts!
            .OrderByDescending(d => d.From) // sort discounts in ascending order by start date
            .FirstOrDefault(d => d.From <= DateTime.Now && d.To >= DateTime.Now);
        
        return Mapper.Map(activeDiscount);
    }
}