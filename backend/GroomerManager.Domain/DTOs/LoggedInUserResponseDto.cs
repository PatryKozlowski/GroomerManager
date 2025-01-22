using GroomerManager.Domain.Entities;

namespace GroomerManager.Domain.DTOs;

public class LoggedInUserResponseDto
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Initials { get; set; }
}