namespace SmartHome.BusinessLogic.Args;

public class CreateCameraArgs
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ModelNumber { get; set; }

    public List<string>? Photos { get; set; }

    public bool? HasMovementDetection { get; set; }

    public bool? HasPersonDetection { get; set; }

    public bool? IsOutdoor { get; set; }

    public bool? IsIndoor { get; set; }
}
