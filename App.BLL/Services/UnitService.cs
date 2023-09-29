using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class UnitService : BaseEntityService<App.BLL.DTO.Unit, App.Domain.Unit, IUnitRepository >, IUnitService
{
    protected IAppUOW Uow;

    public UnitService(IAppUOW uow, IMapper<Unit, Domain.Unit> mapper) 
        : base(uow.UnitRepository, mapper)
    {
        Uow = uow; 
    }
}  