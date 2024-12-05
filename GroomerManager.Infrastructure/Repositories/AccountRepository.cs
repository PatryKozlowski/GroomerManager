using GroomerManager.Domain.Interface;
using GroomerManager.Domain.Models;
using GroomerManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Infrastructure.Repositories;

public class AccountRepository(ApplicationDbContext applicationDbContext) : IAccountRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<Account?> GetAccountById(int accountId)
    {
        return await _applicationDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
    }

    public async Task<int?> GetAccountIdByUserId(int userId)
    {
        return await _applicationDbContext.AccountUsers
            .Where(au => au.UserId == userId)
            .OrderBy(au => au.Id)
            .Select(au => (int?)au.AccountId)
            .FirstOrDefaultAsync();
    }
}
