using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using DAL.EF.Repositories;

namespace DAL.EF;

public class AppUOW : EFBaseUOW<ApplicationDbContext>, IAppUOW
{
    public AppUOW(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    private ICartRepository? _cartRepository;
    public IOrderRepository? _orderRepository;
    public IPaymentRepository? _paymentRepository;
    public IDiscountRepository? _discountRepository;
    public IProductTypeRepository? _productTypeRepository;
    public IUnitRepository? _unitRepository;
    public IProductRepository? _productRepository;
    public ICartProductRepository? _cartProductRepository;
    public IOrderProductRepository? _orderProductRepository;
    public IPriceRepository? _priceRepository;

    public ICartRepository CartRepository => _cartRepository ??= new CartRepository(UowDbContext);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(UowDbContext);
    public IPaymentRepository PaymentRepository => _paymentRepository ??= new PaymentRepository(UowDbContext);
    public IDiscountRepository DiscountRepository => _discountRepository ??= new DiscountRepository(UowDbContext);
    public IProductTypeRepository ProductTypeRepository => _productTypeRepository ??= new ProductTypeRepository(UowDbContext);
    public IUnitRepository UnitRepository => _unitRepository ??= new UnitRepository(UowDbContext);
    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(UowDbContext);
    public ICartProductRepository CartProductRepository => _cartProductRepository ??= new CartProductRepository(UowDbContext);
    public IOrderProductRepository OrderProductRepository => _orderProductRepository ??= new OrderProductRepository(UowDbContext);
    public IPriceRepository PriceRepository => _priceRepository ??= new PriceRepository(UowDbContext);

}