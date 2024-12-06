using System.ComponentModel.DataAnnotations;

namespace GroomerManager.Domain.DTOs;

public record CreateUserDto(
    [Required][EmailAddress] string Email, 
    [Required][MinLength(8)][RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).+$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
    string Password, 
    [Required][MinLength(8)][RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).+$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one special character.")]
    string RepeatPassword
);