using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Homes;

public class HomeService(IHomeRepository homeRepository, IUserRepository userRepository, IRepository<Location> locationRepository,
    IRepository<Coordinates> coordinatesRepository, IMemberRepository memberRepository, IRepository<Device> deviceRepository,
    IHardwareRepository hardwareRepository, IRepository<Camera> cameraRepository, IRepository<LampHardware> lampHardwareRepository,
    IRepository<SensorHardware> sensorHardwareRepository) : IHomeService
{
    public List<Room> GetRooms(Guid homeId)
    {
        return homeRepository.GetRooms(homeId);
    }

    public Home CreateHome(CreateHomeArgs createHomeArgs)
    {
        var coordinates = new Coordinates(createHomeArgs.Latitude, createHomeArgs.Longitude);
        var location = new Location(createHomeArgs.Address, createHomeArgs.DoorNumber);

        AssertArgsAreValid(createHomeArgs, coordinates, location);

        var newHome = new Home
        {
            OwnerEmail = createHomeArgs.Owner!.Email!,
            MemberCount = (int)createHomeArgs.MemberCount,
            Location = location,
            Coordinates = coordinates,
            Name = createHomeArgs.Name
        };

        homeRepository.Add(newHome);
        AddHomeOwnerAsMember(newHome.Id, createHomeArgs.Owner!);
        return newHome;
    }

    private void AddHomeOwnerAsMember(Guid homeId, User owner)
    {
        AddMember(homeId, owner.Email!);

        var member = GetOrDefaultMemberByHomeAndEmail(homeId, owner.Email!);
        foreach (var permission in Enum.GetValues(typeof(ValidHomePermissions)))
        {
            AddPermissionToMember(homeId, member.UserEmail, permission.ToString().ToUpper());
        }
    }

    private void AssertArgsAreValid(CreateHomeArgs createHomeArgs, Coordinates coordinates, Location location)
    {
        if (createHomeArgs.MemberCount == null)
        {
            throw new ArgumentNullException("MemberCount", "Member count cannot be empty");
        }

        if (!userRepository.Exist(u => u.Email!.ToUpper() == createHomeArgs.Owner!.Email!.ToUpper()))
        {
            throw new NotFoundException("User not found");
        }

        if (locationRepository.Exist(l => l.Address == location.Address && l.DoorNumber == location.DoorNumber))
        {
            throw new InvalidOperationException("Location already in use");
        }

        if (coordinatesRepository.Exist(c => c.Latitude == coordinates.Latitude && c.Longitude == coordinates.Longitude))
        {
            throw new InvalidOperationException("Coordinates already in use");
        }
    }

    public void AssertCameraHasPersonDetectionFeature(Guid hardwareId)
    {
        var hardware = hardwareRepository.GetOrDefault(hw => hw.Id == hardwareId);
        var device = cameraRepository.GetOrDefault(d => d.ModelNumber == hardware.DeviceModelNumber);
        if (!(bool)device.HasPersonDetection)
        {
            throw new InvalidOperationException("Camera does not have person detection feature");
        }
    }

    public void AssertCameraHasMovementDetectionFeature(Guid hardwareId)
    {
        var hardware = hardwareRepository.GetOrDefault(hw => hw.Id == hardwareId);
        var device = cameraRepository.GetOrDefault(d => d.ModelNumber == hardware.DeviceModelNumber);
        if (device == null)
        {
            return;
        }

        if (!(bool)device.HasMovementDetection)
        {
            throw new InvalidOperationException("Camera does not have movement detection feature");
        }
    }

    public List<Member> GetMembers(Guid homeId)
    {
        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException("Home not found");
        }

        return homeRepository.GetMembersByHomeId(homeId);
    }

    public void AddMember(Guid homeId, string userEmail)
    {
        if (userEmail == null)
        {
            throw new ArgumentNullException("UserEmail", "User email cannot be empty");
        }

        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException("Home not found");
        }

        if (!userRepository.Exist(u => u.Email == userEmail))
        {
            throw new NotFoundException("User not found");
        }

        var existingMember = memberRepository.GetOrDefault(m => m.UserEmail.ToUpper() == userEmail.ToUpper() && m.HomeId == homeId);

        if (existingMember != null)
        {
            throw new InvalidOperationException("Member already belongs to that home");
        }

        var userRoles = userRepository.GetRoles(userEmail);

        if (!userRoles.Any(r => r.SystemPermissions.Any(sp => sp.Name == nameof(ValidSystemPermissions.BeHomeMember))))
        {
            throw new InvalidOperationException("User cannot belong to a home");
        }

        var member = new Member { UserEmail = userEmail, HomeId = homeId };
        homeRepository.AddMember(homeId, member);
    }

    public List<Hardware> GetHardwares(Guid homeId, string? roomName = null)
    {
        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException("Home not found");
        }

        return homeRepository.GetHardwares(homeId, roomName);
    }

    public void AddHardware(Guid homeId, string? modelNumber)
    {
        if (modelNumber == null)
        {
            throw new ArgumentNullException("ModelNumber", "Model number cannot be empty");
        }

        var device = deviceRepository.GetOrDefault(d => d.ModelNumber.ToUpper() == modelNumber.ToUpper());

        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }

        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException("Home not found");
        }

        if (device.DeviceTypeName == nameof(ValidDeviceTypes.Lamp))
        {
            var lamp = new LampHardware()
            {
                DeviceModelNumber = device.ModelNumber,
                Name = device.Name,
                HomeId = homeId
            };
            lampHardwareRepository.Add(lamp);
            return;
        }

        if (device.DeviceTypeName == nameof(ValidDeviceTypes.Sensor))
        {
            var sensor = new SensorHardware()
            {
                DeviceModelNumber = device.ModelNumber,
                Name = device.Name,
                HomeId = homeId
            };
            sensorHardwareRepository.Add(sensor);
            return;
        }

        var hw = new Hardware
        {
            DeviceModelNumber = device.ModelNumber,
            Name = device.Name,
            HomeId = homeId
        };
        homeRepository.AddHardware(homeId, hw);
    }

    public HardwareData GetHardwareData(Guid hardwareId)
    {
        var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (hardware == null)
        {
            throw new NotFoundException("Hardware not found");
        }

        var device = deviceRepository.GetOrDefault(d => d.ModelNumber == hardware.DeviceModelNumber);
        if (device == null)
        {
            throw new NotFoundException("Device not found");
        }

        if (device.DeviceTypeName == nameof(ValidDeviceTypes.Lamp))
        {
            var lamp = lampHardwareRepository.GetOrDefault(l => l.Id == hardwareId);
            if (lamp == null)
            {
                throw new NotFoundException("Lamp not found");
            }

            return new HardwareData
            {
                Name = lamp.Name!,
                ModelNumber = lamp.DeviceModelNumber,
                LampIsOn = lamp.IsOn,
                ConnectionStatus = lamp.Connected,
                MainPhoto = device.Photos!.First(),
                DoorSensorIsOpen = false,
                Id = hardware.Id,
                IsInARoom = lamp.RoomId != null,
                DeviceType = device.DeviceTypeName
            };
        }

        if (device.DeviceTypeName == nameof(ValidDeviceTypes.Sensor))
        {
            var sensor = sensorHardwareRepository.GetOrDefault(s => s.Id == hardwareId);
            if (sensor == null)
            {
                throw new NotFoundException("Sensor not found");
            }

            return new HardwareData
            {
                Name = sensor.Name!,
                ModelNumber = sensor.DeviceModelNumber,
                LampIsOn = false,
                ConnectionStatus = sensor.Connected,
                MainPhoto = device.Photos!.First(),
                DoorSensorIsOpen = sensor.IsOpen,
                Id = hardware.Id,
                IsInARoom = sensor.RoomId != null,
                DeviceType = device.DeviceTypeName
            };
        }

        return new HardwareData()
        {
            Name = hardware.Name!,
            ModelNumber = hardware.DeviceModelNumber,
            ConnectionStatus = hardware.Connected,
            MainPhoto = device.Photos!.First(),
            LampIsOn = false,
            DoorSensorIsOpen = false,
            Id = hardware.Id,
            IsInARoom = hardware.RoomId != null,
            DeviceType = device.DeviceTypeName
        };
    }

    public List<HardwareData> GetHardwaresAsHardwareData(Guid homeId, string? roomName = null)
    {
        var hardwares = GetHardwares(homeId, roomName);
        var hardwaresData = new List<HardwareData>();

        foreach (var hardware in hardwares)
        {
            var hardwareData = GetHardwareData(hardware.Id);
            hardwaresData.Add(hardwareData);
        }

        return hardwaresData;
    }

    public void ExistHardwareOrThrow(Guid? hardwareId)
    {
        if (hardwareId == null)
        {
            throw new ArgumentNullException("Hardware Id cannot be empty");
        }

        var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);

        if (hardware == null)
        {
            throw new NotFoundException("Hardware not found");
        }
    }

    public void SendNotificationIfSensorStateChanged(Guid hardwareId, string message, bool opened)
    {
        var sensorHardware = sensorHardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (sensorHardware == null)
        {
            throw new NotFoundException("Hardware not found");
        }

        if (sensorHardware.IsOpen != opened)
        {
            SendNotification(hardwareId, message);
        }
        else
        {
            throw new InvalidOperationException("Sensor state did not change");
        }

        sensorHardware.IsOpen = opened;
        sensorHardwareRepository.Update(sensorHardware);
    }

    public void SendNotificationIfLampStateChanged(Guid hardwareId, string message, bool on)
    {
        var lampHardware = lampHardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (lampHardware == null)
        {
            throw new NotFoundException("Hardware not found");
        }

        if (lampHardware.IsOn != on)
        {
            SendNotification(hardwareId, message);
        }
        else
        {
            throw new InvalidOperationException("lamp state did not change");
        }

        lampHardware.IsOn = on;
        lampHardwareRepository.Update(lampHardware);
    }

    public void SendNotification(Guid hardwareId, string message)
    {
        var home = homeRepository.GetOrDefault(h => h.Hardwares.Any(hw => hw.Id == hardwareId));
        if (home == null)
        {
            throw new NotFoundException("Home not found");
        }

        var notification = new Notification
        {
            Message = message,
            HardwareId = hardwareId
        };

        homeRepository.SendNotification(home, notification);
    }

    public void AssertIsValidDevice(Guid hardwareId, string deviceType)
    {
        var hardware = hardwareRepository.GetOrDefault(hw => hw.Id == hardwareId);
        var hardwareType = hardware.Device.DeviceTypeName;
        Console.WriteLine($"expected type: {deviceType}");
        Console.WriteLine($"hardware type: {hardwareType}");

        if (hardwareType != deviceType)
        {
            throw new InvalidOperationException("Device type does not match");
        }
    }

    public bool IsHomeOwner(Guid homeId, string userEmail)
    {
        var home = homeRepository.GetOrDefault(h => h.Id == homeId);

        if (home == null)
        {
            throw new NotFoundException("Home not found");
        }

        return home.OwnerEmail == userEmail;
    }

    public List<NotiAction> GetUserNotificationsWithFilters(GetUserNotificationsArgs getUserNotificationsArgs)
    {
        var userMembers = memberRepository.GetAll(m => m.UserEmail == getUserNotificationsArgs.LoggedUser.Email);

        if (userMembers == null)
        {
            throw new NotFoundException("Memberships not found");
        }

        userMembers = userMembers.Where(m => m.HasHomePermission(nameof(ValidHomePermissions.ReceiveNotifications))).ToList();
        var memberIds = userMembers.Select(m => m.Id).ToList();

        var notifications = homeRepository.GetUserNotificationsWithFilters(getUserNotificationsArgs, memberIds);

        return notifications;
    }

    public void ReadNotifications(List<Guid> readedNotifications, User loggedUser)
    {
        var userMember = memberRepository.GetAll(m => m.UserEmail == loggedUser.Email);
        var notifications = userMember.Where(m => m.NotiActions.Any()).SelectMany(m => m.NotiActions).ToList();
        var notificationsToUpdate = notifications.Where(n => readedNotifications.Contains(n.NotificationId)).ToList();

        homeRepository.ReadNotifications(notificationsToUpdate);
    }

    public void AddPermissionToMember(Guid homeId, string? memberEmail, string? homePermissionName)
    {
        if (memberEmail == null)
        {
            throw new ArgumentNullException("MemberEmail", "Member email cannot be empty");
        }

        if (homePermissionName == null)
        {
            throw new ArgumentNullException("HomePermission", "Home permission cannot be empty");
        }

        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException("Home not found");
        }

        var member = homeRepository.GetOrDefaultMemberByHomeAndEmail(homeId, memberEmail);

        if (member == null)
        {
            throw new NotFoundException("Member not found");
        }

        var homePermission = GetHomePermissionOrThrow(homePermissionName, member.Id);

        if (member.HasHomePermission(homePermissionName))
        {
            throw new InvalidOperationException("Member already has that home permission");
        }

        homeRepository.AddPermissionToMember(member.Id, homePermission);
    }

    public void AddPermissionsToMember(Guid homeId, string? memberEmail, List<string> homePermissions)
    {
        foreach (var homePemrission in homePermissions)
        {
            AddPermissionToMember(homeId, memberEmail, homePemrission);
        }
    }

    public HomePermission GetHomePermissionOrThrow(string permission, Guid memberId)
    {
        if (Enum.TryParse(typeof(ValidHomePermissions), permission, true, out var validHomePermissionName))
        {
            return new HomePermission { Name = validHomePermissionName.ToString(), MemberId = memberId };
        }

        throw new NotFoundException("Home permission not found");
    }

    public Member? GetOrDefaultMemberByHomeAndEmail(Guid homeID, string email)
    {
        var member = homeRepository.GetOrDefaultMemberByHomeAndEmail(homeID, email);

        return member;
    }

    public Room AddRoom(CreateRoomArgs roomArgs)
    {
        if (roomArgs == null)
        {
            throw new ArgumentNullException("RoomArgs", "Room arguments cannot be null");
        }

        if (string.IsNullOrEmpty(roomArgs.Name))
        {
            throw new ArgumentNullException("Name", "Room name cannot be empty");
        }

        if (roomArgs.HardwareIds == null)
        {
            throw new ArgumentNullException("HardwareIds", "Hardware IDs list cannot be null");
        }

        if (roomArgs.HomeId == null)
        {
            throw new ArgumentNullException("HomeId", "Home ID cannot be empty");
        }

        var hardwares = GetHardwaresByIds(roomArgs.HardwareIds);
        ValidateHardwaresBelongToHome(roomArgs.HomeId, roomArgs.HardwareIds);

        var room = new Room
        {
            Name = roomArgs.Name,
            HomeId = (Guid)roomArgs.HomeId,
            Hardwares = hardwares
        };

        ArgumentNullException.ThrowIfNull(room);

        var home = homeRepository.GetOrDefault(h => h.Id == room.HomeId);
        if (home == null)
        {
            throw new NotFoundException($"Home not found.");
        }

        var rooms = homeRepository.GetRooms(home.Id);
        foreach (var existingRoom in rooms)
        {
            foreach (var hardware in existingRoom.Hardwares)
            {
                if (room.Hardwares.Any(h => h.Id == hardware.Id))
                {
                    throw new InvalidOperationException("Hardware already exists in another room in the same home.");
                }
            }
        }

        homeRepository.AddRoom(room);
        return room;
    }

    public List<Hardware> GetHardwaresByIds(List<Guid> hardwareIds)
    {
        if (hardwareIds == null)
        {
            throw new ArgumentNullException("HardwareIds", "Hardware IDs list cannot be null");
        }

        var hardwares = new List<Hardware>();

        foreach (var hardwareId in hardwareIds)
        {
            var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
            if (hardware == null)
            {
                throw new NotFoundException($"Hardware with ID {hardwareId} not found");
            }

            hardwares.Add(hardware);
        }

        return hardwares;
    }

    public void ValidateHardwaresBelongToHome(Guid homeId, List<Guid> hardwareIds)
    {
        foreach (var hardwareId in hardwareIds)
        {
            var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
            if (hardware == null || hardware.HomeId != homeId)
            {
                throw new InvalidOperationException($"Hardware with ID {hardwareId} does not belong to home with ID {homeId}");
            }
        }
    }

    public void SetHardwareRoom(Guid roomId, Guid hardwareId, User loggedUser)
    {
        var room = homeRepository.GetRoom(roomId);
        if (room == null)
        {
            throw new NotFoundException($"Room not found");
        }

        var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (hardware == null)
        {
            throw new NotFoundException($"Hardware not found");
        }

        var home = room.Home;
        if (!home.Hardwares.Any(h => h.Id == hardwareId) && !home.Rooms.Any(r => r.Hardwares.Any(h => h.Id == hardwareId)))
        {
            throw new InvalidOperationException($"Hardware does not belong to home");
        }

        if (room.Hardwares.Any(h => h.Id == hardwareId))
        {
            throw new InvalidOperationException($"Hardware already belongs to room");
        }

        homeRepository.SetHardwareRoom(room, hardware);
    }

    public bool HasHomePermission(string memberEmail, string permission)
    {
        var member = memberRepository.GetOrDefault(m => m.UserEmail.ToUpper() == memberEmail.ToUpper());

        if (member == null)
        {
            throw new NotFoundException("Member not found");
        }

        return member.HasHomePermission(permission);
    }

    public void SetHardwareName(Guid hardwareId, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");
        }

        homeRepository.SetHardwareName(hardwareId, name);
    }

    public void SetHomeName(Guid homeId, string name)
    {
        if (!homeRepository.Exist(h => h.Id == homeId))
        {
            throw new NotFoundException($"Home not found.");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty");
        }

        homeRepository.SetHomeName(homeId, name);
    }

    public void UpdateHardwareStatus(Guid hardwareId, bool connected)
    {
        var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (hardware == null)
        {
            throw new NotFoundException("Hardware not found");
        }

        hardware.Connected = connected;
        hardwareRepository.Update(hardware);
    }

    public void AssertHardwareIsConnected(Guid hardwareId)
    {
        var hardware = hardwareRepository.GetOrDefault(h => h.Id == hardwareId);
        if (!hardware.Connected)
        {
            throw new InvalidOperationException("Hardware is not connected");
        }
    }

    public List<Home> GetHomesWithFilters(GetUserHomesArgs getUserHomesArgs)
    {
        return homeRepository.GetHomesWithFilters(getUserHomesArgs);
    }

    public void AssertUserLoggedIsHomeOwner(Guid homeId, User userLogged)
    {
        if (!IsHomeOwner(homeId, userLogged.Email!))
        {
            throw new ForbiddenAccessException("Only the owner of the home can add members");
        }
    }

    public List<HomePermission> GetMemberPermissions(Guid homeId, string userEmail)
    {
        var member = homeRepository.GetOrDefaultMemberByHomeAndEmail(homeId, userEmail);

        if (member == null)
        {
            throw new NotFoundException("Member not found");
        }

        return homeRepository.GetMemberPermissions(member.Id);
    }

    public bool UserIsTheHomeOwner(Guid homeId, string userEmail)
    {
        var home = homeRepository.GetOrDefault(h => h.Id == homeId);

        if (home == null)
        {
            throw new NotFoundException("Home not found");
        }

        return home.OwnerEmail == userEmail;
    }
}
