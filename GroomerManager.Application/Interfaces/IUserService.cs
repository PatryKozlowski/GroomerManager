using GroomerManager.Domain.DTOs;

namespace GroomerManager.Application.Interfaces;

public interface IUserService
{
    Task<int> CreateUserWithAccount(CreateUserDto request);
}