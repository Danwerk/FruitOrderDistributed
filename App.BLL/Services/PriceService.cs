using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PriceService:
    BaseEntityService<App.BLL.DTO.Price, App.Domain.Price, IPriceRepository>, IPriceService
{
    protected IAppUOW Uow;

    public PriceService(IAppUOW uow, IMapper<App.BLL.DTO.Price, App.Domain.Price> mapper)
        : base(uow.PriceRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<Price?> GetActivePriceAsync(Guid productId)
    {
        // todo: solve possible null problem for product.prices
        var product = await Uow.ProductRepository.FindAsync(productId);
        var activePrice = product!.Prices!
            .OrderByDescending(p => p.From) // sort discounts in ascending order by start date
            .FirstOrDefault(p => p.From <= DateTime.Now && p.To >= DateTime.Now);
        
        return Mapper.Map(activePrice);
    }
}