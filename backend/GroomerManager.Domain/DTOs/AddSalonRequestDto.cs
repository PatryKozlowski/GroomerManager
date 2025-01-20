using Microsoft.AspNetCore.Http;

namespace GroomerManager.Domain.DTOs;

public class AddSalonRequestDto
{
    public required string Name { get; set; }
    public required IFormFile Logo { get; set; }
}