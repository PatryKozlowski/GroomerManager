namespace GroomerManager.Domain.DTOs;

public class NewClientRequestDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
}