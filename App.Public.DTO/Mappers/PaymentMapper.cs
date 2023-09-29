using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class PaymentMapper : BaseMapper< App.BLL.DTO.Payment, App.Public.DTO.v1.Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}