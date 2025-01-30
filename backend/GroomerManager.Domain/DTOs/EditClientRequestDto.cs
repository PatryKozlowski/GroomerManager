namespace GroomerManager.Domain.DTOs;

public class EditClientRequestDto : NewClientRequestDto
{
    public required Guid Id { get; set; }
}