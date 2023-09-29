using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class UnitMapper : BaseMapper<App.BLL.DTO.Unit, App.Public.DTO.v1.Unit>
{
    public UnitMapper(IMapper mapper) : base(mapper)
    {
    }
}