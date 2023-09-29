using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Domain;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class ProductService :
    BaseEntityService<App.BLL.DTO.Product, App.Domain.Product, IProductRepository>, IProductService
{
    protected IAppUOW Uow;

    public ProductService(IAppUOW uow, IMapper<App.BLL.DTO.Product, App.Domain.Product> mapper)
        : base(uow.ProductRepository, mapper)
    {
        Uow = uow;
    }
    
    public async Task<IEnumerable<App.BLL.DTO.Product>> AllAsync()
    {
        return (await Uow.ProductRepository.AllAsync()).Select(e => Mapper.Map(e))!;
    }
    
    public async Task<App.BLL.DTO.Product?> FirstOrDefaultAsync(Guid id)
    {
        var product = await Uow.ProductRepository.FirstOrDefaultAsync(id);
        return Mapper.Map(product);

    }

}