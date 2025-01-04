using System.ComponentModel.DataAnnotations;

namespace SmartHome.BusinessLogic.DeviceTypes;

public class DeviceType
{
    [Key]
    public string Name { get; set; } = null!;
}
