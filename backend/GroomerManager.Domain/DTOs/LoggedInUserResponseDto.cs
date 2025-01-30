using GroomerManager.Domain.Entities;

namespace GroomerManager.Domain.DTOs;

public class LoggedInUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string FullName { get; set; }
}