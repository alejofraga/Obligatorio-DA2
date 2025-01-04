using System.Diagnostics.CodeAnalysis;

using System.Text.Json.Serialization;

namespace JsonImporter;
[ExcludeFromCodeCoverage]
public class JsonDeviceDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("tipo")]
    public string? Tipo { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("modelo")]
    public string? Modelo { get; set; }

    [JsonPropertyName("fotos")]
    public List<PhotoDto>? Fotos { get; set; }

    [JsonPropertyName("person_detection")]
    public bool? PersonDetection { get; set; }

    [JsonPropertyName("movement_detection")]
    public bool? MovementDetection { get; set; }
}
