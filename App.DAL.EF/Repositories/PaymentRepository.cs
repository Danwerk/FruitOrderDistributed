using App.Contracts.DAL;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class PaymentRepository : EFBaseRepository<Payment, ApplicationDbContext>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public override async Task<Payment?> FindAsync(Guid id)
    {
        return await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public override async Task<IEnumerable<Payment>> AllAsync()
    {
        return await RepositoryDbSet.ToListAsync();
    }


    public async Task<IEnumerable<Payment>> AllAsync(Guid userId)
    {
        return await RepositoryDbSet
            .Include(p => p.Orders)
            .Where(p => p.Orders.Any(o => o.AppUserId == userId))
            .ToListAsync();
    }
}