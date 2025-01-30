namespace GroomerManager.Domain.DTOs;

public class ClientNoteResponseDto
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
    public required string CreatedBy { get; set; } = string.Empty;
    public required DateTimeOffset Created { get; set; }
}