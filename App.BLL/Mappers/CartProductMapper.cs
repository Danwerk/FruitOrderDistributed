using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CartProductMapper: BaseMapper<BLL.DTO.CartProduct, App.Domain.CartProduct>
{
    public CartProductMapper(IMapper mapper) : base(mapper)
    {
    }
}