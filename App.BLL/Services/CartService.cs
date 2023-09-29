using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CartService :
    BaseEntityService<App.BLL.DTO.Cart, App.Domain.Cart, ICartRepository>, ICartService
{
    protected IAppUOW Uow;

    public CartService(IAppUOW uow, IMapper<App.BLL.DTO.Cart, App.Domain.Cart> mapper)
        : base(uow.CartRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<Cart>> AllAsync(Guid userId)
    {
        return (await Uow.CartRepository.AllAsync(userId)).Select(e => Mapper.Map(e))!;
    }

    public async Task<Cart?> FindAsync(Guid id, Guid userId)
    {
        return Mapper.Map(await Uow.CartRepository.FindAsync(id, userId));
    }
}