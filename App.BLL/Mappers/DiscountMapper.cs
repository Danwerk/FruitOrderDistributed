using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class DiscountMapper: BaseMapper<BLL.DTO.Discount, App.Domain.Discount>
{
    public DiscountMapper(IMapper mapper) : base(mapper)
    {
    }
}