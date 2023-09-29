using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class PriceMapper : BaseMapper<App.BLL.DTO.Price, App.Public.DTO.v1.Price>
{
    public PriceMapper(IMapper mapper) : base(mapper)
    {
    }
}