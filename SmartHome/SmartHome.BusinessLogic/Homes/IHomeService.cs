using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Homes;

public interface IHomeService
{
    List<NotiAction> GetUserNotificationsWithFilters(GetUserNotificationsArgs getUserNotificationsArgs);
    List<Room> GetRooms(Guid homeId);
    Home CreateHome(CreateHomeArgs createHomeArgs);
    void AddMember(Guid homeId, string userEmail);
    List<Member> GetMembers(Guid homeId);
    void AddPermissionToMember(Guid homeId, string? memberEmail, string? homePermissionName);
    void AddHardware(Guid homeId, string? modelNumber);
    void SendNotification(Guid hardwareId, string message);
    void SendNotificationIfSensorStateChanged(Guid hardwareId, string message, bool opened);
    void SendNotificationIfLampStateChanged(Guid hardwareId, string message, bool on);
    Member? GetOrDefaultMemberByHomeAndEmail(Guid homeID, string email);
    void ExistHardwareOrThrow(Guid? hardwareId);
    void ReadNotifications(List<Guid> readedNotifications, User loggedUser);
    void AssertIsValidDevice(Guid hardwareId, string deviceTypes);
    Room AddRoom(CreateRoomArgs roomArgs);
    List<Hardware> GetHardwaresByIds(List<Guid> hardwareIds);
    void SetHardwareRoom(Guid roomId, Guid hardwareId, User loggedUser);
    void SetHardwareName(Guid hardwareId, string name);
    void SetHomeName(Guid homeId, string? name);
    void AssertCameraHasPersonDetectionFeature(Guid hardwareId);
    void AssertCameraHasMovementDetectionFeature(Guid hardwareId);
    void UpdateHardwareStatus(Guid hardwareId, bool connected);
    void AssertHardwareIsConnected(Guid hardwareId);
    List<Home> GetHomesWithFilters(GetUserHomesArgs getUserHomesArgs);
    void AssertUserLoggedIsHomeOwner(Guid hardwareId, User userLogged);
    List<HardwareData> GetHardwaresAsHardwareData(Guid homeId, string? roomName);
    void AddPermissionsToMember(Guid homeId, string? memberEmail, List<string> homePermissionNames);
    List<HomePermission> GetMemberPermissions(Guid homeId, string userEmail);
    bool UserIsTheHomeOwner(Guid homeId, string userEmail);
}
