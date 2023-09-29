using App.Domain.Identity;
using AutoMapper;

namespace App.Public.DTO;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<App.BLL.DTO.Cart, App.Public.DTO.v1.Cart>()
            .ForMember(dest => dest.TotalPriceIncludingVat, 
                options => options
                    .MapFrom(src =>src.TotalPrice * (decimal)1.2)).ReverseMap();
        
        CreateMap<App.BLL.DTO.CartProduct, App.Public.DTO.v1.CartProduct>().ReverseMap();
        CreateMap<App.BLL.DTO.Discount, App.Public.DTO.v1.Discount>().ReverseMap();
        CreateMap<App.BLL.DTO.Order, App.Public.DTO.v1.Order>().ReverseMap();
        CreateMap<App.BLL.DTO.OrderProduct, App.Public.DTO.v1.OrderProduct>().ReverseMap();
        CreateMap<App.BLL.DTO.Payment, App.Public.DTO.v1.Payment>().ReverseMap();
        CreateMap<App.BLL.DTO.Price, App.Public.DTO.v1.Price>().ReverseMap();
        CreateMap<App.BLL.DTO.Product, App.Public.DTO.v1.Product>().ReverseMap();
        CreateMap<App.BLL.DTO.ProductType, App.Public.DTO.v1.ProductType>().ReverseMap();
        CreateMap<App.BLL.DTO.Unit, App.Public.DTO.v1.Unit>().ReverseMap();
    }
}