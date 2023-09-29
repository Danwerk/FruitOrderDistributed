using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CartProductService :
    BaseEntityService<App.BLL.DTO.CartProduct, App.Domain.CartProduct, ICartProductRepository>, ICartProductService
{
    protected IAppUOW Uow;

    public CartProductService(IAppUOW uow, IMapper<App.BLL.DTO.CartProduct, App.Domain.CartProduct> mapper)
        : base(uow.CartProductRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<CartProduct>> AllAsync(Guid userId)
    {
        return (await Uow.CartProductRepository.AllAsync(userId)).Select(e => Mapper.Map(e))!;
    }
}