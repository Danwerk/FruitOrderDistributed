using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ProductTypeService:
    BaseEntityService<App.BLL.DTO.ProductType, App.Domain.ProductType, IProductTypeRepository>, IProductTypeService
{
    protected IAppUOW Uow;

    public ProductTypeService(IAppUOW uow, IMapper<App.BLL.DTO.ProductType, App.Domain.ProductType> mapper)
        : base(uow.ProductTypeRepository, mapper)
    {
        Uow = uow;
    }
}