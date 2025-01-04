using FluentAssertions;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class HomeRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private UserRepository _userRepository = null!;
    private HomeRepository _homeRepository = null!;
    private Repository<Member> _memberRepository = null!;
    private Repository<HomePermission> _homePermissionRepository = null!;
    private Repository<Device> _deviceRepository = null!;
    private Repository<Hardware> _hardwareRepository = null!;
    private Repository<Company> _companyRepository = null!;
    private Repository<Notification> _notificationRepository = null!;
    private Repository<NotiAction> _notiActionRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _userRepository = new UserRepository(_context);
        _homeRepository = new HomeRepository(_context);
        _memberRepository = new Repository<Member>(_context);
        _homePermissionRepository = new Repository<HomePermission>(_context);
        _companyRepository = new Repository<Company>(_context);
        _deviceRepository = new Repository<Device>(_context);
        _hardwareRepository = new Repository<Hardware>(_context);
        _notificationRepository = new Repository<Notification>(_context);
        _notiActionRepository = new Repository<NotiAction>(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetMembers_WhenHomeExists_ShouldGetMembers()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        var member = GetValidMember(user, home);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _memberRepository.Add(member);

        var homeMembers = _homeRepository.GetMembersByHome(home);

        homeMembers.Should().NotBeNull();
        homeMembers.Should().HaveCount(1);
        homeMembers.First().UserEmail.Should().Be(user.Email);
    }

    [TestMethod]
    public void GetMembersById_WhenHomeExists_ShouldGetMembers()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        var member = GetValidMember(user, home);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _memberRepository.Add(member);

        var homeMembers = _homeRepository.GetMembersByHomeId(home.Id);

        homeMembers.Should().NotBeNull();
        homeMembers.Should().HaveCount(1);
        homeMembers.First().UserEmail.Should().Be(user.Email);
    }

    [TestMethod]
    public void GetHardwares_WhenHomeExists_ShouldGetHardwares()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _companyRepository.Add(GetValidCompany(user));
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);

        var hardwares = _homeRepository.GetHardwares(home.Id);

        hardwares.Should().NotBeNull();
        hardwares.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetHardwaresWithFilters_WhenHomeExists_ShouldGetHardwares()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _companyRepository.Add(GetValidCompany(user));
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        var roomName = "Living room";
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);

        var hardwares = _homeRepository.GetHardwares(home.Id, roomName);

        hardwares.Should().NotBeNull();
        hardwares.Should().HaveCount(0);
    }

    [TestMethod]
    public void GetHardwaresWithFilters_WhenHomeNotFound_ShouldReturnEmptyList()
    {
        var hardwares = _homeRepository.GetHardwares(Guid.NewGuid());
        hardwares.Should().NotBeNull();
        hardwares.Should().HaveCount(0);
    }

    [TestMethod]
    public void AddHardware_WhenHomeExists_ShouldAddHardware()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _companyRepository.Add(GetValidCompany(user));
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        _homeRepository.AddHardware(home.Id, hardware);

        var hardwares = _homeRepository.GetHardwares(home.Id);

        hardwares.Should().NotBeNull();
        hardwares.Should().HaveCount(1);
    }

    [TestMethod]
    public void SendNotification_WhenHomeExistsAndMemberHasPermission_ShouldSendNotification()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 1;
        var home = GetValidHome(user, expectedMemberCount);
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        var member = GetValidMember(user, home);
        var notification = new Notification()
        {
            Message = "Somebody opened the door",
            HardwareId = hardware.Id,
            NotiActions = []
        };
        var homePermission = new HomePermission()
        {
            Name = ValidHomePermissions.ReceiveNotifications.ToString().ToUpper(),
            MemberId = member.Id
        };
        var location = GetValidLocation();
        var coordinates = GetValidCoordinates();
        _userRepository.Add(user);
        location.HomeId = home.Id;
        coordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _memberRepository.Add(member);
        _companyRepository.Add(GetValidCompany(user));
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);
        _homePermissionRepository.Add(homePermission);
        _homeRepository.AddPermissionToMember(member.Id, homePermission);
        _homeRepository.AddPermissionToMember(member.Id, homePermission);

        _homeRepository.SendNotification(home, notification);

        var notiActions = _homeRepository.GetUserNotificationsWithFilters(new GetUserNotificationsArgs(), [member.Id]);
        notiActions.Should().HaveCount(1);
        notification.NotiActions.Should().NotBeNull();
    }

    [TestMethod]
    public void GetUserNotificationsWithFilters_WhenInfoIsCorrect_ShouldGetMemberNotifications()
    {
        var user = GetValidUser();
        var home = GetValidHome(user, 1);
        var member = new Member() { UserEmail = user.Email, HomeId = home.Id };
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        var firstNotification = new Notification { Message = "Message", HardwareId = hardware.Id };
        var secondNotification = new Notification { Message = "Message", HardwareId = hardware.Id };
        var homePermission = new HomePermission
        {
            Name = nameof(ValidHomePermissions.ReceiveNotifications),
            MemberId = member.Id
        };
        _userRepository.Add(user);
        _homeRepository.Add(home);
        _homeRepository.AddMember(home.Id, member);
        _homePermissionRepository.Add(homePermission);
        _companyRepository.Add(GetValidCompany(user));
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);
        _homeRepository.SendNotification(home, firstNotification);
        _homeRepository.SendNotification(home, secondNotification);
        var notiAction = member.NotiActions.FirstOrDefault(na => na.NotificationId == firstNotification.Id);
        notiAction.IsRead = true;
        _context.SaveChanges();
        var getMemberNotificationArgs = new GetUserNotificationsArgs { Read = true, LoggedUser = GetValidUser(), };

        var act = _homeRepository.GetUserNotificationsWithFilters(getMemberNotificationArgs, [member.Id]);

        act.Should().Contain(member.NotiActions.First());
        act.Should().NotContain(member.NotiActions.Last());
        member.Home.Should().NotBeNull();
    }

    [TestMethod]
    public void AddMember_WhenHomeExists_ShouldAddMember()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        var member = GetValidMember(user, home);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _homeRepository.AddMember(home.Id, member);

        var homeMembers = _homeRepository.GetMembersByHomeId(home.Id);

        homeMembers.Should().NotBeNull();
        homeMembers.Should().HaveCount(1);
        homeMembers.First().UserEmail.Should().Be(user.Email);
    }

    [TestMethod]
    public void ReadNotifications_WhenNotificationsAreUnread_ShouldReadNotifications()
    {
        var notiActions = new List<NotiAction>()
        {
            new NotiAction() { IsRead = false, MemberId = Guid.NewGuid(), NotificationId = Guid.NewGuid() },
            new NotiAction() { IsRead = false, MemberId = Guid.NewGuid(), NotificationId = Guid.NewGuid() },
            new NotiAction() { IsRead = false, MemberId = Guid.NewGuid(), NotificationId = Guid.NewGuid() },
        };

        _homeRepository.ReadNotifications(notiActions);

        notiActions.Should().AllSatisfy(notiAction => notiAction.IsRead.Should().BeTrue());
    }

    [TestMethod]
    public void GetOrDefaultMemberByUserEmailAndHomeId_WhenMemberExists_ShouldGetMember()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var expectedLocation = GetValidLocation();
        var expectedCoordinates = GetValidCoordinates();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        var member = GetValidMember(user, home);
        expectedLocation.HomeId = home.Id;
        expectedCoordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _homeRepository.AddMember(home.Id, member);

        var memberFound = _homeRepository.GetOrDefaultMemberByHomeAndEmail(home.Id, user.Email!);

        memberFound.Should().NotBeNull();
        memberFound.UserEmail.Should().Be(user.Email);
    }

    [TestMethod]
    public void AddPermissionToMember_WhenHomeExists_ShouldAddPermissionToMember()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 1;
        var home = GetValidHome(user, expectedMemberCount);
        var location = GetValidLocation();
        var coordinates = GetValidCoordinates();
        var member = GetValidMember(user, home);
        var homePermission =
            new HomePermission() { Name = nameof(_homeRepository.SendNotification), MemberId = member.Id };
        _userRepository.Add(user);
        location.HomeId = home.Id;
        coordinates.HomeId = home.Id;
        _homeRepository.Add(home);
        _memberRepository.Add(member);

        _homeRepository.AddPermissionToMember(member.Id, homePermission);

        var memberUpdated = _homeRepository.GetMembersByHome(home).First();
        memberUpdated.HomePermissions.Should().NotBeNull();
        memberUpdated.HomePermissions.Should().HaveCount(1);
    }

    [TestMethod]
    public void AddRoom_WhenHomeExists_ShouldAddRoom()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        _homeRepository.Add(home);
        var room = new Room() { Name = "Living room", Hardwares = [], HomeId = home.Id };

        _homeRepository.AddRoom(room);

        var homeWithRoom = _homeRepository.GetOrDefault(h => h.Id == home.Id);
        homeWithRoom.Should().NotBeNull();
    }

    [TestMethod]
    public void AddRoom_WhenRoomWithSameNameExists_ShouldThrow()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        _homeRepository.Add(home);
        var room = new Room() { Name = "Living room", Hardwares = [], HomeId = home.Id };

        _homeRepository.AddRoom(room);

        var act = () => _homeRepository.AddRoom(room);
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetRooms_WhenHomeExists_ShouldGetRooms()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        _homeRepository.Add(home);
        var room = new Room() { Name = "Living room", Hardwares = [], HomeId = home.Id };
        _homeRepository.AddRoom(room);

        var rooms = _homeRepository.GetRooms(home.Id);

        rooms.Should().NotBeNull();
        rooms.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetRoom_WhenRoomExists_ShouldGetRoom()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        _homeRepository.Add(home);
        var room = new Room() { Name = "Living room", Hardwares = [], HomeId = home.Id };
        _homeRepository.AddRoom(room);

        var roomFound = _homeRepository.GetRoom(room.Id);

        roomFound.Should().NotBeNull();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenRoomExists_ShouldAddHardwareToRoom()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 3;
        var home = GetValidHome(user, expectedMemberCount);
        var room = new Room() { Name = "Living room", Hardwares = [], HomeId = home.Id };
        var company = GetValidCompany(user);
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        _userRepository.Add(user);
        _homeRepository.Add(home);
        _companyRepository.Add(company);
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);
        _homeRepository.AddRoom(room);

        _homeRepository.SetHardwareRoom(room, hardware);

        var roomWithHardware = _homeRepository.GetRoom(room.Id);
        roomWithHardware.Hardwares.Should().NotBeNull();
        roomWithHardware.Hardwares.Should().HaveCount(1);
    }

    [TestMethod]
    public void SetHardwareName_WhenHardwareExists_ShouldSetHardwareName()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 1;
        var home = GetValidHome(user, expectedMemberCount);
        var member = GetValidMember(user, home);
        var device = GetValidDevice(user);
        var hardware = GetValidHardware(device, home.Id);
        _userRepository.Add(user);
        _companyRepository.Add(GetValidCompany(user));
        _deviceRepository.Add(device);
        _homeRepository.Add(home);
        _memberRepository.Add(member);
        _hardwareRepository.Add(hardware);
        var newName = "NewName";

        _homeRepository.SetHardwareName(hardware.Id, newName);

        var hardwareUpdated = _homeRepository.GetHardwares(home.Id).First();
        hardwareUpdated.Name.Should().Be(newName);
    }

    [TestMethod]
    public void GetHomesWithFilters_WhenHomesExist_ShouldGetHomes()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 1;
        var home = GetValidHome(user, expectedMemberCount);
        _userRepository.Add(user);
        _homeRepository.Add(home);
        var getHomesArgs = new GetUserHomesArgs() { User = user, Limit = 3, Offset = 2 };

        var homes = _homeRepository.GetHomesWithFilters(getHomesArgs);

        homes.Should().NotBeNull();
        homes.Should().HaveCount(0);
    }

    [TestMethod]
    public void GetUserNotificationsWithFilters_ShouldReturnNotifications_WhenNotificationsExist()
    {
        var user = new User
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Test User",
            Lastname = "fraga",
            Password = "@Gag54271928"
        };
        var company = GetValidCompany(user);
        var home = GetValidHome(user, 5);
        var member = new Member { UserEmail = user.Email, HomeId = home.Id };
        var device = new Device
        {
            ModelNumber = "111",
            DeviceTypeName = "Sensor",
            Description = "-",
            CompanyRUT = company.RUT,
            Name = "Test Device",
            Photos = ["photo1.jpg"]
        };
        var hardware = new Hardware { HomeId = home.Id, DeviceModelNumber = device.ModelNumber };
        var notification = new Notification { Message = " / ", HardwareId = hardware.Id, Date = DateTime.Now };
        var notiAction = new NotiAction
        {
            NotificationId = notification.Id,
            MemberId = member.Id,
            IsRead = false,
            HomeId = home.Id
        };
        _userRepository.Add(user);
        _homeRepository.Add(home);
        _memberRepository.Add(member);
        _companyRepository.Add(company);
        _deviceRepository.Add(device);
        _hardwareRepository.Add(hardware);
        _notificationRepository.Add(notification);
        _notiActionRepository.Add(notiAction);
        _context.SaveChanges();

        var getUserNotificationsArgs = new GetUserNotificationsArgs
        {
            DateTime = notification.Date.ToString(),
            Read = false,
            DeviceType = "Sensor"
        };
        var result =
            _homeRepository.GetUserNotificationsWithFilters(getUserNotificationsArgs,
                [notiAction.MemberId]);
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }

    [TestMethod]
    public void GetUserNotificationsWithFilters_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        var getUserNotificationsArgs = new GetUserNotificationsArgs
        {
            DateTime = DateTime.Now.ToString(),
            Read = false,
            DeviceType = "Sensor"
        };

        var result =
            _homeRepository.GetUserNotificationsWithFilters(getUserNotificationsArgs,
                [Guid.NewGuid()]);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void SetHomeName_WhenHomeExists_ShouldSetHomeName()
    {
        var user = GetValidUser();
        const int expectedMemberCount = 1;
        var home = GetValidHome(user, expectedMemberCount);
        _userRepository.Add(user);
        _homeRepository.Add(home);

        _homeRepository.SetHomeName(home.Id, "HomeName");

        var homeUpdated = _homeRepository.GetOrDefault(h => h.Id == home.Id);
        homeUpdated!.Name.Should().Be("HomeName");
    }

    [TestMethod]
    public void GetMemberPermissions_WhenMemberExists_ShouldReturnPermissions()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var home = GetValidHome(user, 1);
        var member = GetValidMember(user, home);
        var homePermission = new HomePermission { Name = "PermissionName", MemberId = member.Id };
        _homeRepository.Add(home);
        _memberRepository.Add(member);
        _homePermissionRepository.Add(homePermission);

        var permissions = _homeRepository.GetMemberPermissions(member.Id);

        permissions.Should().NotBeNull();
        permissions.Should().HaveCount(1);
        permissions.First().Name.Should().Be("PermissionName");
    }

    [TestMethod]
    public void GetMemberById_WhenMemberExists_ShouldReturnMember()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var home = GetValidHome(user, 1);
        var member = GetValidMember(user, home);
        _homeRepository.Add(home);
        _memberRepository.Add(member);

        var result = _homeRepository.GetMemberById(member.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(member.Id);
    }

    #region SampleData

    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456",
            ProfilePicturePath = "pathMati"
        };
    }

    private Company GetValidCompany(User user)
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = user.Email
        };
    }

    private Member GetValidMember(User user, Home home)
    {
        return new Member() { UserEmail = user.Email, HomeId = home.Id };
    }

    private Home GetValidHome(User user, int memberCount)
    {
        return new Home()
        {
            Owner = user,
            Coordinates = GetValidCoordinates(),
            Location = GetValidLocation(),
            OwnerEmail = user.Email,
            MemberCount = memberCount,
        };
    }

    private Hardware GetValidHardware(Device device, Guid homeId)
    {
        return new Hardware()
        {
            DeviceModelNumber = device.ModelNumber,
            HomeId = homeId,
            Device = device,
            Connected = true,
        };
    }

    private Location GetValidLocation()
    {
        const string expectedAddress = "Golden street";
        const string doorNumber = "818";
        return new Location(expectedAddress, doorNumber);
    }

    private Device GetValidDevice(User user)
    {
        return new Device()
        {
            ModelNumber = "5",
            CompanyRUT = GetValidCompany(user).RUT,
            Name = "Device",
            Description = "Description",
            Photos = ["www.photo.com"],
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
    }

    private Coordinates GetValidCoordinates()
    {
        const string latitude = "123";
        const string longitude = "456";
        return new Coordinates(latitude, longitude);
    }

    #endregion
}
