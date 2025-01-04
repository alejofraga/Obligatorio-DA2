using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public readonly struct GetUserHomesResponse(Home home)
{
    public readonly Guid Id { get; init; } = home.Id;

    public readonly string Name { get; init; } = home.Name!;
}
