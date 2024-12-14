using GroomerManager.Domain.Entities;

namespace GroomerManager.Domain.DTOs;

public record UserDto(
    int Id,
    string Email,
    ICollection<UserRole> Roles
    );