using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Base.Contracts.DAL;

public interface IBaseUOW
{
    Task<int> SaveChangesAsync();
    // ?? how to contain and create repositories
}