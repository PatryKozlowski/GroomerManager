using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GroomerManager.Domain.DTOs;

public class AddSalonRequestDto
{
    public required string Name { get; set; }
    [FromForm]
    public required IFormFile Logo { get; set; }
}