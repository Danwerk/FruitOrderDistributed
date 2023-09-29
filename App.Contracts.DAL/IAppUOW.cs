using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IBaseUOW
{
    // List your repositories here
    ICartRepository CartRepository { get; }
    IOrderRepository OrderRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IDiscountRepository DiscountRepository { get; }
    IProductTypeRepository ProductTypeRepository { get; }
    IUnitRepository UnitRepository { get; }
    IProductRepository ProductRepository { get; }
    ICartProductRepository CartProductRepository { get; }
    IOrderProductRepository OrderProductRepository { get; }
    IPriceRepository PriceRepository { get; }
    
}