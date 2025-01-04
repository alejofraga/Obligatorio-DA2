using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.DataLayer;

public class HomeRepository(DbContext context) : Repository<Home>(context), IHomeRepository
{
    private readonly DbSet<Home> _homes = context.Set<Home>();
    private readonly DbSet<Member> _members = context.Set<Member>();
    private readonly DbSet<Hardware> _hardwares = context.Set<Hardware>();
    private readonly DbSet<Location> _locations = context.Set<Location>();
    private readonly DbSet<Coordinates> _coordinates = context.Set<Coordinates>();
    private readonly DbSet<NotiAction> _notiActions = context.Set<NotiAction>();
    private readonly DbSet<Notification> _notifications = context.Set<Notification>();
    private readonly DbSet<Room> _rooms = context.Set<Room>();
    private readonly DbContext _context = context;

    public List<Member> GetMembersByHomeId(Guid homeId)
    {
        var home = _homes.Include(h => h.Members)
            .ThenInclude(m => m.User)
            .Include(h => h.Members)
            .ThenInclude(m => m.HomePermissions)
            .FirstOrDefault(h => h.Id == homeId);
        return home.Members.ToList();
    }

    public List<Hardware> GetHardwares(Guid homeId, string? roomName = null)
    {
        var home = _homes.Include(h => h.Hardwares)
            .ThenInclude(hw => hw.Device)
            .Include(h => h.Hardwares)
            .ThenInclude(hw => hw.Room)
            .FirstOrDefault(h => h.Id == homeId);

        if (home == null)
        {
            return [];
        }

        var hardwares = home.Hardwares.AsQueryable();

        if (!string.IsNullOrEmpty(roomName))
        {
            hardwares = hardwares.Where(hw => hw.Room != null && hw.Room.Name == roomName);
        }

        return hardwares.ToList();
    }

    public void AddHardware(Guid homeId, Hardware hardware)
    {
        var home = _homes.Include(h => h.Hardwares).FirstOrDefault(h => h.Id == homeId);
        home!.Hardwares.Add(hardware);

        hardware.HomeId = homeId;
        _hardwares.Add(hardware);
        _context.SaveChanges();
    }

    public void AddMember(Guid homeId, Member member)
    {
        var home = GetOrDefault(h => h.Id == homeId);
        var members = GetMembersByHomeId(homeId);
        if (members.Count + 1 > home!.MemberCount)
        {
            throw new InvalidOperationException("Home is full");
        }

        member.HomeId = homeId;
        _members.Add(member);
        _context.SaveChanges();
    }

    public List<Member> GetMembersByHome(Home home)
    {
        var homeWithMembers = _homes.Include(h => h.Members)
            .ThenInclude(m => m.HomePermissions)
            .FirstOrDefault(h => h.Id == home.Id);
        return homeWithMembers.Members.ToList();
    }

    public void SendNotification(Home home, Notification notification)
    {
        _notifications.Add(notification);

        var members = GetMembersByHome(home);

        foreach (var member in members)
        {
            var canReceiveNotifications = member.HomePermissions.Any(hp =>
                hp.Name.ToUpper() == ValidHomePermissions.ReceiveNotifications.ToString().ToUpper());

            if (canReceiveNotifications)
            {
                var notiAction = new NotiAction
                {
                    NotificationId = notification.Id,
                    MemberId = member.Id,
                    IsRead = false,
                    HomeId = home.Id
                };
                _notiActions.Add(notiAction);
            }
        }

        _context.SaveChanges();
    }

    public List<NotiAction> GetUserNotificationsWithFilters(GetUserNotificationsArgs getUserNotificationsArgs,
        List<Guid> memberIds)
    {
        var query = _notiActions.Where(na => memberIds.Contains(na.MemberId))
            .Include(na => na.Notification)
            .ThenInclude(n => n.Hardware)
            .ThenInclude(h => h.Device)
            .AsQueryable();

        if (!string.IsNullOrEmpty(getUserNotificationsArgs.DateTime))
        {
            if (DateTime.TryParse(getUserNotificationsArgs.DateTime, out DateTime parsedDateTime))
            {
                query = query.Where(na => na.Notification.Date.Date == parsedDateTime.Date);
            }
        }

        if (getUserNotificationsArgs.Read != null)
        {
            query = query.Where(na => na.IsRead == getUserNotificationsArgs.Read);
        }

        if (!string.IsNullOrEmpty(getUserNotificationsArgs.DeviceType))
        {
            query = query.Where(na =>
                na.Notification.Hardware.Device.DeviceTypeName == getUserNotificationsArgs.DeviceType);
        }

        return query.ToList();
    }

    public void AddPermissionToMember(Guid memberId, HomePermission homePermission)
    {
        var memberToUpdate = _members.FirstOrDefault(m => m.Id == memberId);
        memberToUpdate.HomePermissions.Add(homePermission);
        _context.SaveChanges();
    }

    public void ReadNotifications(List<NotiAction> notifications)
    {
        notifications.ForEach(n => n.IsRead = true);

        context.SaveChanges();
    }

    public Member? GetOrDefaultMemberByHomeAndEmail(Guid homeID, string email)
    {
        var member = _members.Include(m => m.HomePermissions)
            .FirstOrDefault(m => m.HomeId == homeID && m.UserEmail == email);

        return member;
    }

    public new void Add(Home home)
    {
        _homes.Add(home);
        _locations.Add(home.Location);
        _coordinates.Add(home.Coordinates);
        context.SaveChanges();
    }

    public void AddRoom(Room room)
    {
        var roomWIthSameNameExists = GetOrDefault(h => h.Rooms.Where(r => r.Name == room.Name && r.HomeId == room.HomeId).Any()) != null;
        if (roomWIthSameNameExists)
        {
            throw new InvalidOperationException("Room with the same name already exists");
        }

        _rooms.Add(room);
        context.SaveChanges();
    }

    public List<Room> GetRooms(Guid homeId)
    {
        var rooms = _rooms
            .Where(r => r.HomeId == homeId)
            .Include(r => r.Hardwares)
            .Include(r => r.Home)
            .ToList();
        return rooms;
    }

    public Room? GetRoom(Guid roomId)
    {
        return _rooms
            .Include(r => r.Hardwares)
            .Include(r => r.Home)
            .FirstOrDefault(r => r.Id == roomId);
    }

    public void SetHardwareRoom(Room room, Hardware hardware)
    {
        hardware.Room = room;
        context.SaveChanges();
    }

    public void SetHardwareName(Guid hardwareId, string name)
    {
        var hardware = _hardwares.FirstOrDefault(h => h.Id == hardwareId);
        hardware.Name = name;
        _context.SaveChanges();
    }

    public void SetHomeName(Guid homeId, string name)
    {
        var home = _homes.FirstOrDefault(h => h.Id == homeId);
        home!.Name = name;
        _context.SaveChanges();
    }

    public List<Home> GetHomesWithFilters(GetUserHomesArgs getUserHomesArgs)
    {
        var query = _homes.Where(h =>
            h.Members.Any(m => m.User.Email == getUserHomesArgs.User.Email) ||
            h.OwnerEmail == getUserHomesArgs.User.Email).AsQueryable();

        query = query.Skip<Home>(getUserHomesArgs.Offset)
            .Take<Home>(getUserHomesArgs.Limit);

        return query.ToList<Home>();
    }

    public List<HomePermission> GetMemberPermissions(Guid memberId)
    {
        var member = _members.Include(m => m.HomePermissions).FirstOrDefault(m => m.Id == memberId);
        var permissions = member!.HomePermissions;
        return permissions;
    }

    public Member GetMemberById(Guid memberId)
    {
        return _members.FirstOrDefault(m => m.Id == memberId);
    }
}
