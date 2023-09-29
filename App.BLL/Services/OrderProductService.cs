using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class OrderProductService :
    BaseEntityService<App.BLL.DTO.OrderProduct, App.Domain.OrderProduct, IOrderProductRepository>, IOrderProductService
{
    protected IAppUOW Uow;

    public OrderProductService(IAppUOW uow, IMapper<App.BLL.DTO.OrderProduct, App.Domain.OrderProduct> mapper)
        : base(uow.OrderProductRepository, mapper)
    {
        Uow = uow;
    }
    public async Task<IEnumerable<OrderProduct>> AllAsync(Guid userId)
    {
        return (await Uow.OrderProductRepository.AllAsync(userId)).Select(e => Mapper.Map(e))!;
    }
}