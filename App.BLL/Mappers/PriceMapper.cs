using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PriceMapper: BaseMapper<BLL.DTO.Price, App.Domain.Price>
{
    public PriceMapper(IMapper mapper) : base(mapper)
    {
    }
}