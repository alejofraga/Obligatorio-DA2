using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Importer;

namespace JsonImporter;
[ExcludeFromCodeCoverage]
public class JsonImporterImplementation : IImporter
{
    public List<DeviceDto> Import(Dictionary<string, string> parameters)
    {
        var route = parameters["path"];
        var data = File.ReadAllText(route);
        RootObject rootObject;
        try
        {
            rootObject = JsonSerializer.Deserialize<RootObject>(data);
        }
        catch (Exception e)
        {
            throw new InvalidCastException("Couldnt deserealize the json file en el jsonImporter", e);
        }

        var deviceDtos = rootObject.Dispositivos.Select(jsonDevice => new DeviceDto()
        {
            DeviceType = jsonDevice.Tipo switch
            {
                "sensor-open-close" => "Sensor",
                "sensor-movement" => "MovementSensor",
                "camera" => "Camera",
                "lamp" => "Lamp",
                _ => jsonDevice.Tipo
            },
            Name = jsonDevice.Nombre,
            ModelNumber = jsonDevice.Modelo,
            HasPersonDetection = jsonDevice.PersonDetection,
            HasMovementDetection = jsonDevice.MovementDetection,
            Photos = jsonDevice.Fotos?
                .OrderByDescending(photo => photo.EsPrincipal)
                .Select(photo => photo.Path)
                .ToList()
        }).ToList();

        return deviceDtos;
    }

    public Dictionary<string, string> GetImporterParams()
    {
        var parameters = new Dictionary<string, string> { { "path", "string" } };
        return parameters;
    }
}

public class RootObject
{
    [JsonPropertyName("dispositivos")]
    public List<JsonDeviceDto> Dispositivos { get; set; } = [];
}
