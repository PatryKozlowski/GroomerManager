using System.Text.Json.Serialization;

namespace GroomerManager.Shared.DTOs.Response;

public class ErrorResponse
{
    [property: JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("errors")]
    public List<FieldError>? Errors { get; set; }
}

public class FieldError
{
    [JsonPropertyName("fieldName")]
    public string FieldName { get; set; } = string.Empty;

    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;
}