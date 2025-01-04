using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;

namespace SmartHome.DataLayer;

public class DeviceRepository(SmartHomeDbContext context) : Repository<Device>(context), IDeviceRepository
{
    private readonly DbSet<Device> _devices = context.Set<Device>();

    public List<Device> GetDevicesWithFilters(GetDevicesArgs getDevicesArgs)
    {
        var query = _devices.Include(d => d.Company).AsQueryable();

        if (!string.IsNullOrEmpty(getDevicesArgs.ModelNumber))
        {
            var upperModelNumber = getDevicesArgs.ModelNumber.ToUpper();
            query = query.Where(d => d.ModelNumber.ToUpper().StartsWith(upperModelNumber));
        }

        if (!string.IsNullOrEmpty(getDevicesArgs.DeviceName))
        {
            var upperDeviceName = getDevicesArgs.DeviceName.ToUpper();
            query = query.Where(d => d.Name.ToUpper().StartsWith(upperDeviceName));
        }

        if (!string.IsNullOrEmpty(getDevicesArgs.CompanyName))
        {
            var upperCompanyName = getDevicesArgs.CompanyName.ToUpper();
            query = query.Where(d => d.Company != null && d.Company.Name.ToUpper().StartsWith(upperCompanyName));
        }

        if (!string.IsNullOrEmpty(getDevicesArgs.DeviceType))
        {
            var upperDeviceType = getDevicesArgs.DeviceType.ToUpper();
            query = query.Where(d => d.DeviceTypeName.ToUpper().StartsWith(upperDeviceType));
        }

        query = query.Skip(getDevicesArgs.Offset)
            .Take(getDevicesArgs.Limit);

        return query.ToList();
    }
}
