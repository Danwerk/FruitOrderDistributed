using AutoMapper;

namespace App.BLL;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<BLL.DTO.Order, App.Domain.Order>().ReverseMap();
        CreateMap<BLL.DTO.OrderProduct, App.Domain.OrderProduct>().ReverseMap();
        CreateMap<BLL.DTO.Payment, App.Domain.Payment>().ReverseMap();
        CreateMap<BLL.DTO.CartProduct, App.Domain.CartProduct>().ReverseMap();
        CreateMap<BLL.DTO.Product, App.Domain.Product>().ReverseMap();
        CreateMap<BLL.DTO.Cart, App.Domain.Cart>().ReverseMap();
        CreateMap<BLL.DTO.Unit, App.Domain.Unit>().ReverseMap();
        CreateMap<BLL.DTO.Discount, App.Domain.Discount>().ReverseMap();
        CreateMap<BLL.DTO.Price, App.Domain.Price>().ReverseMap();
        CreateMap<BLL.DTO.ProductType, App.Domain.ProductType>().ReverseMap();
    }
}