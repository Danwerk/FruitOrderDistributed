using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBaseBLL
{
    IOrderService OrderService { get; }
    ICartProductService CartProductService { get; }

    IUnitService UnitService { get; }

    IProductService ProductService { get; }

    ICartService CartService { get; }

    IDiscountService DiscountService { get; }

    IOrderProductService OrderProductService { get; }

    IPaymentService PaymentService { get; }
    
    IPriceService PriceService { get; }
    
    IProductTypeService ProductTypeService { get; }
}