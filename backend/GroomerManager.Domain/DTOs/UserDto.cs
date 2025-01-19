using GroomerManager.Domain.Entities;

namespace GroomerManager.Domain.DTOs;

public class UserDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required Role Role { get; set; }
}