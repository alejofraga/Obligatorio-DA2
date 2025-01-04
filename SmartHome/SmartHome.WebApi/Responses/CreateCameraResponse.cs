using SmartHome.BusinessLogic.Devices;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateCameraResponse(Camera camera)
{
    public readonly string ModelNumber { get; init; } = camera.ModelNumber!;

    public readonly string Name { get; init; } = camera.Name!;

    public readonly string Description { get; init; } = camera.Description!;

    public readonly List<string> Photos { get; init; } = camera.Photos!;

    public readonly bool HasMovementDetection { get; init; } = (bool)camera.HasMovementDetection!;

    public readonly bool HasPersonDetection { get; init; } = (bool)camera.HasPersonDetection!;

    public readonly bool IsOutdoor { get; init; } = (bool)camera.IsOutdoor!;

    public readonly bool IsIndoor { get; init; } = (bool)camera.IsIndoor!;
}
