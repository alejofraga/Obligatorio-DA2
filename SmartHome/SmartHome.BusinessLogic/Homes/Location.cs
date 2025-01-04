namespace SmartHome.BusinessLogic.Homes;

public class Location()
{
    public string? Address { get; set; }
    public string? DoorNumber { get; set; }
    public Home? Home { get; set; }
    public Guid HomeId { get; set; }

    public Location(string? address, string? doorNumber)
        : this()
    {
        AssertIsNotEmpty(address, "Address", "Address");
        Address = address;
        AssertIsNotEmpty(doorNumber, "DoorNumber", "Door number");
        DoorNumber = doorNumber;
    }

    private static void AssertIsNotEmpty(string? value, string attributeName, string attributeAlias)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(attributeName, $"{attributeAlias} cannot be empty");
        }
    }
}
