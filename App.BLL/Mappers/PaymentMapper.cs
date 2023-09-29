using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class PaymentMapper : BaseMapper<BLL.DTO.Payment, App.Domain.Payment>
{
    public PaymentMapper(IMapper mapper) : base(mapper)
    {
    }
}