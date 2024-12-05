using GroomerManager.Domain.Models;

namespace GroomerManager.Domain.Interface;

public interface IAccountRepository
{
    Task<int?> GetAccountIdByUserId(int userId);
    Task<Account?> GetAccountById(int accountId);
}
