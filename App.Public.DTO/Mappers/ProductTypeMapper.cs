using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class ProductTypeMapper : BaseMapper<App.BLL.DTO.ProductType, App.Public.DTO.v1.ProductType>
{
    public ProductTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}