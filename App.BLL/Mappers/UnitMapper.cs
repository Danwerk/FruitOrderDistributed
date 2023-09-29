using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UnitMapper : BaseMapper<BLL.DTO.Unit, App.Domain.Unit>
{
    public UnitMapper(IMapper mapper) : base(mapper)
    {
    }
}