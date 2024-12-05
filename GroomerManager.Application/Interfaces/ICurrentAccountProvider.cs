using GroomerManager.Domain.Models;

namespace GroomerManager.Application.Interfaces;

public interface ICurrentAccountProvider
{
    Task<Account> GetAuthenticatedAccount();
    Task<int?> GetAccountId();
}
