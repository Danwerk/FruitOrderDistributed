using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class OrderService : 
    BaseEntityService<App.BLL.DTO.Order, App.Domain.Order, IOrderRepository>, IOrderService
{
    protected IAppUOW Uow;

    public OrderService(IAppUOW uow, IMapper<App.BLL.DTO.Order, App.Domain.Order> mapper) 
        : base(uow.OrderRepository,mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<Order>> AllAsync(Guid userId)
    {
        return (await Uow.OrderRepository.AllAsync(userId)).Select(e => Mapper.Map(e))!;
    }

    public async Task<Order?> FindAsync(Guid id, Guid userId)
    {
        return Mapper.Map(await Uow.OrderRepository.FindAsync(id, userId));
    }
}
