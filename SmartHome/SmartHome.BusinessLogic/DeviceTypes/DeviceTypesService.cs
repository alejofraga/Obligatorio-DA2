namespace SmartHome.BusinessLogic.DeviceTypes;

public class DeviceTypesService(IRepository<DeviceType> deviceTypeRepository) : IDeviceTypesService
{
    public List<DeviceType> GetDeviceTypes()
    {
        return deviceTypeRepository.GetAll();
    }
}
