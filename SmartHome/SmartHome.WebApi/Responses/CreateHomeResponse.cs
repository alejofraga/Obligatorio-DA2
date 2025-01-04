using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateHomeResponse(Home home)
{
    public readonly string Name { get; init; } = home.Name!;

    public readonly string Owner { get; init; } = home.OwnerEmail!;

    public readonly string Address { get; init; } = home.Location.Address!;

    public readonly string DoorNumber { get; init; } = home.Location.DoorNumber!;

    public readonly string Latitude { get; init; } = home.Coordinates.Latitude!;

    public readonly string Longitude { get; init; } = home.Coordinates.Longitude!;

    public readonly int MemberCount { get; init; } = home.MemberCount;
}
