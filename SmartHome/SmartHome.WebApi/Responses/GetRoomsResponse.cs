using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public class GetRoomsResponse()
{
    public GetRoomsResponse(List<Room> rooms)
        : this()
    {
        Rooms = rooms.Select(room => new RoomData(room)).ToList();
    }

    public List<RoomData> Rooms { get; set; } = null!;
}

public class RoomData(Room room)
{
    public string Name { get; set; } = room.Name;
    public Guid Id { get; set; } = room.Id;
}
