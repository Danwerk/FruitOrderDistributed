using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class ProductMapper : BaseMapper<BLL.DTO.Product, App.Domain.Product>
{
    public ProductMapper(IMapper mapper) : base(mapper)
    {
    }
}