using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Devices;

public interface IDeviceService
{
    List<Device> GetDevicesWithFilters(GetDevicesArgs getDevicesArgs);
    Camera AddCamera(CreateCameraArgs args, User user);
    Device AddDevice(CreateBasicDeviceArgs args, User user);
    List<string> GetImporterNames();
    Dictionary<string, string> GetImporterParams(string importerName);
    void ImportDevices(string importerName, Dictionary<string, string> parameters, User user);
}
