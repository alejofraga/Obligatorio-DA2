namespace SmartHome.BusinessLogic.Homes;

public class HardwareData
{
    public string Name { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string MainPhoto { get; set; } = null!;
    public bool ConnectionStatus { get; set; }
    public bool LampIsOn { get; set; }
    public bool DoorSensorIsOpen { get; set; }
    public bool IsInARoom { get; set; }

    public string DeviceType { get; set; } = null!;

    public Guid Id { get; set; }
}
