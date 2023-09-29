using App.Contracts.DAL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IUnitService : IBaseRepository<App.BLL.DTO.Unit>, IUnitRepositoryCustom<App.BLL.DTO.Unit>
{
    // add your custom service methods here
}