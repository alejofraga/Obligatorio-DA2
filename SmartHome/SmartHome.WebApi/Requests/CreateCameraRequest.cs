namespace SmartHome.WebApi.Requests;

public class CreateCameraRequest
{
    public string? Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public string? ModelNumber { get; set; } = null!;
    public List<string>? Photos { get; set; } = null!;

    public bool? HasMovementDetection { get; set; } = null!;

    public bool? HasPersonDetection { get; set; } = null!;

    public bool? IsOutdoor { get; set; } = null!;

    public bool? IsIndoor { get; set; } = null!;
}
