namespace GroomerManager.Domain.DTOs;

public class AddSalonResponseDto
{
    public required Guid SalonId { get; set; }
    public required string LogoPath { get; set; }
}