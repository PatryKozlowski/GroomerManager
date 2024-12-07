using GroomerManager.Domain.Interface;
using GroomerManager.Domain.Models;
using GroomerManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext applicationDbContext) : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    public async Task<bool> IsUserExist(string userEmail)
    {
        return await _applicationDbContext.Users.AnyAsync(u => u.Email == userEmail);
    }
    public async Task<User> CreateUserWithAccount(string email, string password)
    {
        var user = new User
            {
                Email = email,
                HasPassword = password,
                CreatedBy = email
            };

        await _applicationDbContext.Users.AddAsync(user);

        var account = new Account
        {
            Name = email,
            CreatedBy = email
        };

        await _applicationDbContext.AddAsync(account);

        var accountUser = new AccountUser
        {
            Account = account,
            User = user,
            CreatedBy = email
        };

        await _applicationDbContext.AddAsync(accountUser);
        
        await _applicationDbContext.SaveChangesAsync();

        return user;
    }
    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user ?? null;
    }
}