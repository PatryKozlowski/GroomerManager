using GroomerManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GroomerManager.Infrastructure.Auth;

public class PasswordManager(IPasswordHasher<PasswordManager.DummyUser> passwordHasher) : IPasswordManager
{
    private readonly IPasswordHasher<DummyUser> _passwordHasher = passwordHasher;
    
    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(new DummyUser(), password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var verificationResult = _passwordHasher.VerifyHashedPassword(new DummyUser(), hash, password);
        return (verificationResult == PasswordVerificationResult.Success ||
                verificationResult == PasswordVerificationResult.SuccessRehashNeeded);
    }
    public class DummyUser
    {

    }
}