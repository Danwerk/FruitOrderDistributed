using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class PaymentService :
    BaseEntityService<App.BLL.DTO.Payment, App.Domain.Payment, IPaymentRepository>, IPaymentService
{
    protected IAppUOW Uow;

    public PaymentService(IAppUOW uow, IMapper<App.BLL.DTO.Payment, App.Domain.Payment> mapper)
        : base(uow.PaymentRepository, mapper)
    {
        Uow = uow;
    }


    
    
}