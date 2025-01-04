using SmartHome.BusinessLogic.DeviceTypes;

namespace SmartHome.WebApi.Responses;

public readonly struct GetDeviceTypesResponse(DeviceType deviceType)
{
    public readonly string Name { get; init; } = deviceType.Name;
}
