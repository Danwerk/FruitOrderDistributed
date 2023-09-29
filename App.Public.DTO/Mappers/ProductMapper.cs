using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class ProductMapper : BaseMapper<App.BLL.DTO.Product, App.Public.DTO.v1.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}