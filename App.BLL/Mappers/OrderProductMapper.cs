using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class OrderProductMapper: BaseMapper<BLL.DTO.OrderProduct, App.Domain.OrderProduct>
{
    public OrderProductMapper(IMapper mapper) : base(mapper)
    {
    }
}