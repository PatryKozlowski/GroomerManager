namespace GroomerManager.Domain.DTOs;

public class ClientsResponseDto
{
   public required List<ClientResponseDto> Clients { get; set; } = [];
   public required int TotalCount { get; set; }
   public required int PageCount { get; set; }
}