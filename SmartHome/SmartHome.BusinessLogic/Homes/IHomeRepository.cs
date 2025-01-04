using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Homes;

public interface IHomeRepository : IRepository<Home>
{
    List<Member> GetMembersByHomeId(Guid homeId);
    List<Member> GetMembersByHome(Home home);
    List<Hardware> GetHardwares(Guid homeId, string? roomName);
    void AddHardware(Guid homeId, Hardware hardware);
    void AddMember(Guid homeId, Member member);
    void SendNotification(Home home, Notification notification);
    void AddPermissionToMember(Guid memberId, HomePermission homePermission);
    void ReadNotifications(List<NotiAction> notifications);
    Member? GetOrDefaultMemberByHomeAndEmail(Guid homeID, string email);
    void AddRoom(Room room);
    List<Room> GetRooms(Guid homeId);
    Room? GetRoom(Guid roomId);
    void SetHardwareRoom(Room room, Hardware hardware);
    void SetHardwareName(Guid hardwareId, string name);
    void SetHomeName(Guid homeId, string name);
    List<NotiAction> GetUserNotificationsWithFilters(GetUserNotificationsArgs getMemberNotificationsArgs, List<Guid> memberIds);
    List<Home> GetHomesWithFilters(GetUserHomesArgs getUserHomesArgs);
    List<HomePermission> GetMemberPermissions(Guid memberId);
}
