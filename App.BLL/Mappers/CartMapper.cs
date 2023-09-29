using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CartMapper: BaseMapper<BLL.DTO.Cart, App.Domain.Cart>
{
    public CartMapper(IMapper mapper) : base(mapper)
    {
    }
}