namespace GroomerManager.Domain.DTOs;

public class ClientResponseDto
{
        public required Guid Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
}