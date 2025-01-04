using SmartHome.BusinessLogic.Devices;

namespace SmartHome.WebApi.Responses;

public readonly struct GetDevicesResponse(Device device)
{
    public readonly string Name { get; init; } = device.Name!;

    public readonly string CompanyName { get; init; } = device.Company!.Name!;

    public readonly string ModelNumber { get; init; } = device.ModelNumber!;

    public readonly string MainPhoto { get; init; } = device.Photos!.First();

    public readonly string DeviceType { get; init; } = device.DeviceTypeName!;
}
