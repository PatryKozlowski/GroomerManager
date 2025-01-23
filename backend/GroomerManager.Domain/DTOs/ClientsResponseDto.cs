namespace GroomerManager.Domain.DTOs;

public class ClientsResponseDto
{
   public required List<ClientsDto> Clients { get; set; } = new List<ClientsDto>();
   public required int TotalCount { get; set; }
   public required int PageCount { get; set; }
}

public class ClientsDto
{
   public required Guid Id { get; set; }
   public required string FirstName { get; set; } = string.Empty;
   public required string LastName { get; set; } = string.Empty;
   public required string PhoneNumber { get; set; } = string.Empty;
   public string? Email { get; set; }
}