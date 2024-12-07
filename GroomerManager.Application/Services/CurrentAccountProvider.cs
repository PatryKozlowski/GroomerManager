using GroomerManager.Application.Interfaces;
using GroomerManager.Domain.Interface;
using GroomerManager.Domain.Models;

namespace GroomerManager.Application.Services;

public class CurrentAccountProvider(
    IAuthenticationDataProvider authenticationDataProvider,
    IAccountRepository accountRepository
    ) : ICurrentAccountProvider
{
    private readonly IAuthenticationDataProvider _authenticationDataProvider = authenticationDataProvider;
    private readonly IAccountRepository _accountRepository = accountRepository;
    public async Task<int?> GetAccountId()
    {
        var userId = _authenticationDataProvider.GetUserId();

        if (userId != null)
        {
            return await _accountRepository.GetAccountIdByUserId(userId.Value);
        }

        return null;
    }

    public async Task<Account> GetAuthenticatedAccount()
    {
        var accountId = await GetAccountId();

        if (accountId == null)
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }

        var account = await _accountRepository.GetAccountById(accountId.Value);

        if (account == null)
        {
            throw new Exception("AccountDoesNotExist");
        }

        return account;
    }
}
