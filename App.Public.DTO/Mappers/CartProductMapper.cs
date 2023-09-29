using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class CartProductMapper : BaseMapper<App.BLL.DTO.CartProduct, App.Public.DTO.v1.CartProduct>
{
    public CartProductMapper(IMapper mapper) : base(mapper)
    {
    }
}