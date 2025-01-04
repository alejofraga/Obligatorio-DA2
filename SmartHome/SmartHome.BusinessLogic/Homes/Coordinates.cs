namespace SmartHome.BusinessLogic.Homes;

public class Coordinates()
{
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public Home? Home { get; set; }
    public Guid HomeId { get; set; }

    public Coordinates(string? latitude, string? longitude)
        : this()
    {
        AssertIsNotEmpty(latitude, "Latitude");
        Latitude = latitude;
        AssertIsNotEmpty(longitude, "Longitude");
        Longitude = longitude;
    }

    private static void AssertIsNotEmpty(string? value, string attributeName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(attributeName, $"{attributeName} cannot be empty");
        }
    }
}
