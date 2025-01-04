using Importer;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Devices;

public interface IDeviceImporter
{
    List<DeviceDto> Import(string importerName, Dictionary<string, string> parameters, User user);
    Dictionary<string, string> GetImporterParams(string importerType);
    List<string> GetImporterNames();
}
