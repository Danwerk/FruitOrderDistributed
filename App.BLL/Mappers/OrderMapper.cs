using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class OrderMapper : BaseMapper<BLL.DTO.Order, App.Domain.Order>
{
    public OrderMapper(IMapper mapper) : base(mapper)
    {
    }
}