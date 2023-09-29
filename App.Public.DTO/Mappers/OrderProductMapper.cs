using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class OrderProductMapper : BaseMapper<App.BLL.DTO.OrderProduct, App.Public.DTO.v1.OrderProduct>
{
    public OrderProductMapper(IMapper mapper) : base(mapper)
    {
    }
}