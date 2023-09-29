using App.BLL.Mappers;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    protected IAppUOW Uow;
    private readonly AutoMapper.IMapper _mapper;

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
    }

    private IOrderService? _orders;
    private IOrderProductService? _orderProducts;
    private ICartProductService? _cartProducts;
    private IUnitService? _units;
    private IProductService? _products;
    private ICartService? _carts;
    private IDiscountService? _discounts;
    private IPaymentService? _payments;
    private IPriceService? _prices;
    private IProductTypeService? _productTypes;


    public IOrderService OrderService => _orders ??= new OrderService(Uow, new OrderMapper(_mapper));

    public IOrderProductService OrderProductService =>
        _orderProducts ??= new OrderProductService(Uow, new OrderProductMapper(_mapper));

    public ICartProductService CartProductService =>
        _cartProducts ??= new CartProductService(Uow, new CartProductMapper(_mapper));

    public IUnitService UnitService => _units ??= new UnitService(Uow, new UnitMapper(_mapper));
    public IProductService ProductService => _products ??= new ProductService(Uow, new ProductMapper(_mapper));
    public ICartService CartService => _carts ??= new CartService(Uow, new CartMapper(_mapper));
    public IDiscountService DiscountService => _discounts ??= new DiscountService(Uow, new DiscountMapper(_mapper));
    public IPaymentService PaymentService => _payments ??= new PaymentService(Uow, new PaymentMapper(_mapper));
    public IPriceService PriceService => _prices ??= new PriceService(Uow, new PriceMapper(_mapper));
    public IProductTypeService ProductTypeService => _productTypes ??= new ProductTypeService(Uow, new ProductTypeMapper(_mapper));
}