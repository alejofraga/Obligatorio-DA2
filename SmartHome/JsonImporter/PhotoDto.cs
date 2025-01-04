using System.Diagnostics.CodeAnalysis;

using System.Text.Json.Serialization;

namespace JsonImporter;
[ExcludeFromCodeCoverage]
public class PhotoDto
{
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("es_principal")]
    public bool EsPrincipal { get; set; }
}
