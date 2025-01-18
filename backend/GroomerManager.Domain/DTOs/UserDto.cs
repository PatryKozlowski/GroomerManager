namespace GroomerManager.Domain.DTOs;

public record UserDto(
    int Id,
    string Email,
    List<string> Roles
);