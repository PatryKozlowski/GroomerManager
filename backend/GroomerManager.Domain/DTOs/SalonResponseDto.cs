namespace GroomerManager.Domain.DTOs;

public class SalonResponseDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string LogoPath { get; set; }
}