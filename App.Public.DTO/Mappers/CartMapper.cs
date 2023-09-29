using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class CartMapper : BaseMapper<App.BLL.DTO.Cart, App.Public.DTO.v1.Cart>
{
    public CartMapper(IMapper mapper) : base(mapper)
    {
    }
}