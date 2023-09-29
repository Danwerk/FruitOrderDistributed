using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class DiscountMapper : BaseMapper<App.BLL.DTO.Discount, App.Public.DTO.v1.Discount>
{
    public DiscountMapper(IMapper mapper) : base(mapper)
    {
    }
}