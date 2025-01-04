using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateRoomResponse(Room room)
{
    public readonly string RoomName { get; init; } = room.Name!;
}
