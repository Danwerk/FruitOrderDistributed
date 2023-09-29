using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class OrderMapper : BaseMapper<App.BLL.DTO.Order, App.Public.DTO.v1.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}