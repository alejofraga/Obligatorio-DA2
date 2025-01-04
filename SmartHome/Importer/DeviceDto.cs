using System.Diagnostics.CodeAnalysis;

namespace Importer;
[ExcludeFromCodeCoverage]
public class DeviceDto
{
    public string? DeviceType { get; set; }
    public string? Name { get; set; }
    public string? ModelNumber { get; set; }
    public List<string?>? Photos { get; set; }
    public bool? HasMovementDetection { get; set; }
    public bool? HasPersonDetection { get; set; }
}
