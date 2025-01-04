namespace SmartHome.BusinessLogic.Homes;

public class Room
{
    public Guid Id { get; private init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required List<Hardware> Hardwares { get; set; }
    public required Guid HomeId { get; set; }
    public Home? Home { get; set; }
}
