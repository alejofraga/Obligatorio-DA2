using SmartHome.BusinessLogic.Devices;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateDeviceResponse(Device device)
{
    public readonly string ModelNumber { get; init; } = device.ModelNumber!;

    public readonly string Name { get; init; } = device.Name!;

    public readonly string Description { get; init; } = device.Description!;

    public readonly string[] Photos { get; init; } = device.Photos!.ToArray();

    public readonly string DeviceType { get; init; } = device.DeviceTypeName!;
}
