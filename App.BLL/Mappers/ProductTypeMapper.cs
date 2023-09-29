using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProductTypeMapper: BaseMapper<BLL.DTO.ProductType, App.Domain.ProductType>
{
    public ProductTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}