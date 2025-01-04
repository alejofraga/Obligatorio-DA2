using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Devices;

public interface IDeviceRepository : IRepository<Device>
{
    List<Device> GetDevicesWithFilters(GetDevicesArgs getDevicesArgs);
}
