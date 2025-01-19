namespace GroomerManager.Domain.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    List<string> Roles
);