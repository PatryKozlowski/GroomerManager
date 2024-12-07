using GroomerManager.Application.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Interface;

namespace GroomerManager.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordManager passwordManager
    ) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordManager _passwordManager = passwordManager;
    
    public async Task<int> CreateUserWithAccount(CreateUserDto request)
    {
        var userExists = await _userRepository.IsUserExist(request.Email);

        if (userExists)
        {
            throw new InvalidOperationException("AccountWithThisEmailAlreadyExists");
        }

        if (request.Password != request.RepeatPassword)
        {
            throw new InvalidOperationException("PasswordNotMatch");
        }

        var hashedPassword = _passwordManager.HashPassword(request.Password);

        var user = await _userRepository.CreateUserWithAccount(request.Email, hashedPassword);

        return user.Id;
    }
    public async Task<int> LoginUser(LoginUserDto request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user != null)
        {
            var isValidPassword = _passwordManager.VerifyPassword(user.HasPassword, request.Password);
            
            if (isValidPassword)
            {
                return user.Id;
            }
        }
        
        throw new InvalidOperationException("InvalidLoginOrPassword");
    }
}