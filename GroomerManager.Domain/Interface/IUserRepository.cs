using GroomerManager.Domain.Models;

namespace GroomerManager.Domain.Interface;

public interface IUserRepository
{
    Task<bool> IsUserExist (string userEmail);
    Task<User> CreateUserWithAccount(string  email, string password);
}