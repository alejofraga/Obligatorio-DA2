using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Homes;

[TestClass]
public class HomeService_Test
{
    private static Mock<IHomeRepository> _homeRepositoryMock = null!;
    private static Mock<IUserRepository> _userRepositoryMock = null;
    private static Mock<IRepository<Coordinates>> _coordinatesRepositoryMock = null!;
    private static Mock<IRepository<Location>> _locationRepositoryMock = null!;
    private static Mock<IMemberRepository> _memberRepositoryMock = null;
    private static Mock<IRepository<NotiAction>> _notiActionRepositoryMock = null;
    private static Mock<IRepository<Device>> _deviceRepositoryMock = null!;
    private static Mock<IHardwareRepository> _hardwareRepositoryMock = null!;
    private HomeService _homeService = null!;
    private static Mock<IRepository<Camera>> _cameraRepositoryMock = null!;
    private static Mock<IRepository<LampHardware>> _lampHardwareRepositoryMock = null!;
    private static Mock<IRepository<SensorHardware>> _sensorHardwareRepositoryMock = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _homeRepositoryMock = new Mock<IHomeRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _coordinatesRepositoryMock = new Mock<IRepository<Coordinates>>(MockBehavior.Strict);
        _locationRepositoryMock = new Mock<IRepository<Location>>(MockBehavior.Strict);
        _memberRepositoryMock = new Mock<IMemberRepository>(MockBehavior.Strict);
        _deviceRepositoryMock = new Mock<IRepository<Device>>(MockBehavior.Strict);
        _hardwareRepositoryMock = new Mock<IHardwareRepository>(MockBehavior.Strict);
        _notiActionRepositoryMock = new Mock<IRepository<NotiAction>>(MockBehavior.Strict);
        _lampHardwareRepositoryMock = new Mock<IRepository<LampHardware>>(MockBehavior.Strict);
        _cameraRepositoryMock = new Mock<IRepository<Camera>>(MockBehavior.Strict);
        _sensorHardwareRepositoryMock = new Mock<IRepository<SensorHardware>>(MockBehavior.Strict);

        _homeService = new HomeService(_homeRepositoryMock.Object, _userRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _coordinatesRepositoryMock.Object, _memberRepositoryMock.Object, _deviceRepositoryMock.Object,
            _hardwareRepositoryMock.Object, _cameraRepositoryMock.Object, _lampHardwareRepositoryMock.Object, _sensorHardwareRepositoryMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeRepositoryMock.VerifyAll();
        _userRepositoryMock.VerifyAll();
        _coordinatesRepositoryMock.VerifyAll();
        _locationRepositoryMock.VerifyAll();
        _memberRepositoryMock.VerifyAll();
        _deviceRepositoryMock.VerifyAll();
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
        _lampHardwareRepositoryMock.VerifyAll();
        _sensorHardwareRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void CreateHome_WhenInfoIsCorrect_ShouldCreateHome()
    {
        var owner = GetValidUser();
        var createHomeArgs = new CreateHomeArgs()
        {
            Owner = owner,
            Address = "Golden street",
            DoorNumber = "818",
            Latitude = "123",
            Longitude = "456",
            Name = "Home",
            MemberCount = 3,
        };
        var member = new Member { UserEmail = createHomeArgs.Owner.Email, HomeId = Guid.NewGuid() };
        var createHomePermission = new SystemPermission() { Name = nameof(ValidSystemPermissions.CreateHome) };
        var beHomeMember = new SystemPermission() { Name = nameof(ValidSystemPermissions.BeHomeMember) };
        var homeOwnerRole = new Role { Name = nameof(ValidUserRoles.HomeOwner) };
        homeOwnerRole.SystemPermissions = [createHomePermission, beHomeMember];
        owner.Roles = [homeOwnerRole];

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _memberRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Member, bool>>>()))
            .Returns((Member)null);
        _homeRepositoryMock
            .Setup(hr => hr.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);
        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(true);
        _userRepositoryMock
            .Setup(repo => repo.GetRoles(It.IsAny<string>()))
            .Returns([homeOwnerRole]);
        _coordinatesRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Coordinates, bool>>>()))
            .Returns(false);
        _locationRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Location, bool>>>()))
            .Returns(false);
        _homeRepositoryMock
            .Setup(repo => repo.AddMember(It.IsAny<Guid>(), It.IsAny<Member>()));
        _homeRepositoryMock
            .Setup(repo => repo.AddPermissionToMember(It.IsAny<Guid>(), It.IsAny<HomePermission>()));
        _homeRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Home>()));

        var newHome = _homeService.CreateHome(createHomeArgs);

        newHome.Should().NotBeNull();
        newHome.OwnerEmail.Should().Be(createHomeArgs.Owner.Email);
        newHome.Location.Address.Should().Be(createHomeArgs.Address);
        newHome.Location.DoorNumber.Should().Be(createHomeArgs.DoorNumber);
        newHome.Coordinates.Latitude.Should().Be(createHomeArgs.Latitude);
        newHome.Coordinates.Longitude.Should().Be(createHomeArgs.Longitude);
        newHome.Name.Should().Be(createHomeArgs.Name);
    }

    [TestMethod]
    public void CreateHome_WhenOwnerDoesntExists_ShouldThrowException()
    {
        var createHomeArgs = new CreateHomeArgs()
        {
            Owner = GetValidUser(),
            Address = "Golden street",
            DoorNumber = "818",
            Latitude = "123",
            Longitude = "456",
            Name = "Home",
            MemberCount = 3,
        };

        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(false);

        var act = () => _homeService.CreateHome(createHomeArgs);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddMember_WhenHomeAndUserExists_ShouldAddMember()
    {
        var user = GetValidUser();
        var userEmail = user.Email;
        var home = GetValidHome();
        var homeId = home.Id;
        var beHomeMember = new SystemPermission() { Name = nameof(ValidSystemPermissions.BeHomeMember) };
        var homeOwnerRole = new Role { Name = nameof(ValidUserRoles.HomeOwner) };
        homeOwnerRole.SystemPermissions = [beHomeMember];
        user.Roles = [homeOwnerRole];

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _userRepositoryMock
            .Setup(repo => repo.GetRoles(It.IsAny<string>()))
            .Returns([homeOwnerRole]);
        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(true);
        _memberRepositoryMock
            .Setup(repo => repo.GetOrDefault(m => m.UserEmail.ToUpper() == userEmail.ToUpper() && m.HomeId == homeId))
            .Returns(value: null);
        _homeRepositoryMock
            .Setup(repo => repo.AddMember(It.IsAny<Guid>(), It.IsAny<Member>()));

        var act = () => _homeService.AddMember(home.Id, user.Email!);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AddMember_WhenUserEmailIsNull_ShouldThrowArgumentNullException()
    {
        var act = () => _homeService.AddMember(Guid.NewGuid(), null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddMember_WhenHomeDoesNotExist_ShouldThrowNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(false);

        var act = () => _homeService.AddMember(homeId, userEmail);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddMember_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(false);

        var act = () => _homeService.AddMember(homeId, userEmail);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddMember_WhenMemberAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";
        var member = new Member { UserEmail = userEmail, HomeId = homeId };

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(true);
        _memberRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Member, bool>>>()))
            .Returns(member);

        var act = () => _homeService.AddMember(homeId, userEmail);

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddMember_WhenUserIsNotHomeOwner_ShouldThrowInvalidOperationException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _userRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(true);
        _memberRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Member, bool>>>()))
            .Returns(value: null);
        _userRepositoryMock
            .Setup(repo => repo.GetRoles(userEmail))
            .Returns([new Role { Name = "member" }]);

        var act = () => _homeService.AddMember(homeId, userEmail);

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void AddHardware_WhenHomeAndDeviceExists_ShouldAddDevice()
    {
        var user = GetValidUser();
        var device = GetValidDevice();
        const int expectedMemberCount = 3;
        var home = GetValidHome();

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(device);
        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _sensorHardwareRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<SensorHardware>()));

        _homeService.AddHardware(home.Id, device.ModelNumber);
    }

    [TestMethod]
    public void SendNotification_WhenCalled_ShouldAddNotificationOnlyToMembersWithPermissions()
    {
        var userWithPermission = GetValidUser();
        var device = GetValidDevice();
        var homeId = Guid.NewGuid();
        var hardware = new Hardware { DeviceModelNumber = device.ModelNumber, HomeId = homeId };
        var home = new Home
        {
            OwnerEmail = userWithPermission.Email,
            Owner = userWithPermission,
            Location = new Location("Golden street", "818"),
            Coordinates = new Coordinates("123", "456"),
            MemberCount = 1,
            Hardwares = [hardware]
        };
        var message = "Camera detected motion!";

        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(home);
        _homeRepositoryMock
            .Setup(repo => repo.SendNotification(home, It.IsAny<Notification>()));

        _homeService.SendNotification(hardware.Id, message);
    }

    [TestMethod]
    public void GetMembers_WhenHomeExists_ShouldReturnMembers()
    {
        var members = new List<Member>
        {
            new Member { UserEmail = "user1@example.com", HomeId = Guid.NewGuid() },
            new Member { UserEmail = "user2@example.com", HomeId = Guid.NewGuid() }
        };
        var homeId = Guid.NewGuid();

        _homeRepositoryMock
            .Setup(hr => hr.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(hr => hr.GetMembersByHomeId(It.IsAny<Guid>()))
            .Returns(members);

        var result = _homeService.GetMembers(homeId);

        result.Should().BeEquivalentTo(members);
    }

    [TestMethod]
    public void GetMembers_WhenHomeDoesntExists_ShouldThrow()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock
            .Setup(hr => hr.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(false);

        Action act = () => _homeService.GetMembers(homeId);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void GetHardwares_WhenHomeDoesntExists_ShouldThrow()
    {
        var homeId = Guid.NewGuid();

        _homeRepositoryMock
            .Setup(hr => hr.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(false);

        Action act = () => _homeService.GetHardwares(homeId);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void ExistHardwareOrThrow_WhenHardwareExists_ShouldNotThrow()
    {
        var hardware = new Hardware { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };

        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);

        var act = () => _homeService.ExistHardwareOrThrow(hardware.Id);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void ExistHardwareOrThrow_WhenHardwareDoesntExists_ShouldThrow()
    {
        var hardwareId = Guid.NewGuid();

        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns((Hardware)null);

        var act = () => _homeService.ExistHardwareOrThrow(hardwareId);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AssertIsValidDevice_WhenDeviceTypeIsWrong_ShouldThrow()
    {
        var device = GetValidDevice();
        var hardware = new Hardware { DeviceModelNumber = device.ModelNumber, HomeId = Guid.NewGuid(), Device = device };

        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);

        var act = () => _homeService.AssertIsValidDevice(hardware.Id, nameof(Camera));

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void IsHomeOwner_WhenOk_ShouldNotThrow()
    {
        var home = new Home()
        {
            Coordinates = new Coordinates("123", "456"),
            Location = new Location("Golden street", "818"),
            OwnerEmail = "email@google.com",
            MemberCount = 10,
        };

        _homeRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(home);

        var act = _homeService.IsHomeOwner(home.Id, home.OwnerEmail);

        act.Should().BeTrue();
    }

    [TestMethod]
    public void IsHomeOwner_WhenHomeDoesntExists_ShouldThrow()
    {
        const string anotherEmail = "someother@google.com";
        var homeId = Guid.NewGuid();

        _homeRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns((Home)null);

        var act = () => _homeService.IsHomeOwner(homeId, anotherEmail);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void GetUserNotificationsWithFilters_WhenMemberDoenstHavePermission_ShouldBeEmptyList()
    {
        var member = new Member() { UserEmail = "maticor93@gmail.com", HomeId = Guid.NewGuid() };
        var getUserNotificationsArgs = new GetUserNotificationsArgs
        {
            Read = true,
            LoggedUser = GetValidUser(),
        };

        _memberRepositoryMock
            .Setup(m => m.GetAll(m => m.UserEmail == getUserNotificationsArgs.LoggedUser.Email))
            .Returns([member]);
        _homeRepositoryMock
            .Setup(m => m.GetUserNotificationsWithFilters(getUserNotificationsArgs, It.IsAny<List<Guid>>()))
            .Returns([]);

        var result = _homeService.GetUserNotificationsWithFilters(getUserNotificationsArgs);

        result.Should().BeEmpty();
    }

    [TestMethod]
    public void HasHomePermission_WhenPermissionNameIsNull_ShouldReturnFalse()
    {
        var member = new Member()
        {
            UserEmail = "some",
            HomeId = Guid.NewGuid()
        };

        var act = member.HasHomePermission(null);

        act.Should().BeFalse();
    }

    [TestMethod]
    public void HasHomePermission_WhentTrue_ShouldReturnTrue()
    {
        var member = new Member()
        {
            UserEmail = "some",
            HomeId = Guid.NewGuid(),
            HomePermissions = [new HomePermission { Name = nameof(ValidHomePermissions.ReceiveNotifications), MemberId = Guid.NewGuid() }]
        };

        var act = member.HasHomePermission(nameof(ValidHomePermissions.ReceiveNotifications));

        act.Should().BeTrue();
    }

    [TestMethod]
    public void GetOrDefaultMemberByHomeAndEmail_WhenOk_ShouldGetMember()
    {
        var member = new Member() { HomeId = Guid.NewGuid(), UserEmail = "alejofraga22v2@gmail.com" };

        _homeRepositoryMock
            .Setup(hr => hr.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);

        var act = _homeService.GetOrDefaultMemberByHomeAndEmail(member.HomeId, member.UserEmail);

        act.Should().Be(member);
    }

    [TestMethod]
    public void ReadNotifications_WhenOk_ShouldReadNotifications()
    {
        var loggedUser = GetValidUser();
        var home = GetValidHome();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = home.Id };
        var notification = new Notification { Message = "Camera detected motion!" };
        var firstNotification = new NotiAction()
        {
            IsRead = false,
            MemberId = member.Id,
            HomeId = home.Id,
            NotificationId = notification.Id,
            Home = home,
            Member = member
        };
        var secondNotification = new NotiAction() { IsRead = false, MemberId = member.Id, HomeId = home.Id, NotificationId = notification.Id, };
        var notiActions = new List<NotiAction> { firstNotification, secondNotification };
        member.NotiActions = notiActions;

        _memberRepositoryMock
            .Setup(hr => hr.GetAll(m => m.UserEmail == loggedUser.Email))
            .Returns([member]);
        _homeRepositoryMock
            .Setup(hr => hr.ReadNotifications(notiActions));

        _homeService.ReadNotifications([firstNotification.NotificationId], loggedUser);

        notiActions[0].MemberId.Should().NotBeEmpty();
        notiActions[0].HomeId.Should().NotBeEmpty();
        notiActions[0].NotificationId.Should().NotBeEmpty();
        notiActions[0].Home.Should().NotBeNull();
        notiActions[0].Member.Should().NotBeNull();
    }

    [TestMethod]
    public void AssertIsValidDevice_WhenDeviceExists_ShouldNotThrow()
    {
        var device = GetValidDevice();
        var hardware = new Hardware { DeviceModelNumber = "1", HomeId = Guid.NewGuid(), Device = device };

        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);

        var act = () => _homeService.AssertIsValidDevice(hardware.Id, ValidDeviceTypes.Sensor.ToString());

        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetHardwares_WhenOk_ShouldGetHardwares()
    {
        var homeId = Guid.NewGuid();
        var hardwares = new List<Hardware>
        {
            new Hardware { DeviceModelNumber = "1", HomeId = homeId },
            new Hardware { DeviceModelNumber = "2", HomeId = homeId }
        };

        _homeRepositoryMock
            .Setup(hr => hr.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(hr => hr.GetHardwares(It.IsAny<Guid>(), null))
            .Returns(hardwares);

        var act = _homeService.GetHardwares(homeId);

        act.Should().Contain(hardwares.First());
        act.Should().Contain(hardwares.Last());
    }

    [TestMethod]
    public void AddPermissionToMember_WhenHomeAndUserExists_ShouldAddPermission()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var member = new Member { User = user, UserEmail = user.Email, HomeId = home.Id, HomePermissions = [] };
        var memberId = member.Id;
        var permissionName = ValidHomePermissions.AddDevice.ToString();
        var newPermission = new HomePermission
        {
            Name = permissionName,
            MemberId = member.Id
        };

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);
        _homeRepositoryMock
            .Setup(repo => repo.AddPermissionToMember(It.IsAny<Guid>(), It.IsAny<HomePermission>()));

        var act = () => _homeService.AddPermissionToMember(home.Id, member.UserEmail, "AddDevice");

        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetRooms_WhenHomeHasRooms_ShouldGetRooms()
    {
        var homeId = Guid.NewGuid();
        var expectedRooms = new List<Room>() { new Room() { Name = "this one is it", Hardwares = [], HomeId = Guid.NewGuid() } };
        _homeRepositoryMock.Setup(hr => hr.GetRooms(homeId)).Returns(expectedRooms);
        var result = _homeService.GetRooms(homeId);
        result.Should().BeEquivalentTo(expectedRooms);
    }

    [TestMethod]
    public void AddRoom_WhenHomeExists_ShouldAddRoom()
    {
        var owner = GetValidUser();
        var home = GetValidHome();
        var roomArgs = new CreateRoomArgs()
        {
            Name = "Living room",
            HardwareIds = [],
            HomeId = Guid.NewGuid()
        };

        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(home);
        _homeRepositoryMock
            .Setup(repo => repo.GetRooms(home.Id))
            .Returns([]);
        _homeRepositoryMock
            .Setup(repo => repo.AddRoom(It.IsAny<Room>()));

        _homeService.AddRoom(roomArgs);
    }

    [TestMethod]
    public void AddRoom_WhenHomeDoesntExists_ShouldThrow()
    {
        var roomArgs = new CreateRoomArgs()
        {
            Name = "Living room",
            HardwareIds = [],
            HomeId = Guid.NewGuid()
        };

        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns((Home)null);

        Action act = () => _homeService.AddRoom(roomArgs);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddRoom_WhenRoomIsNull_ShouldThrow()
    {
        Action act = () => _homeService.AddRoom(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void GetHardwaresByIds_WhenHardwareIdsIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _homeService.GetHardwaresByIds(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void GetHardwaresByIds_WhenHardwareIdNotFound_ShouldThrowNotFoundException()
    {
        var hardwareIds = new List<Guid> { Guid.NewGuid() };

        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns((Hardware)null);

        Action act = () => _homeService.GetHardwaresByIds(hardwareIds);

        act.Should().Throw<NotFoundException>().WithMessage($"Hardware with ID {hardwareIds.First()} not found");
    }

    [TestMethod]
    public void GetHardwaresByIds_WhenAllHardwareIdsExist_ShouldReturnHardwares()
    {
        var firstHardware = new Hardware
        {
            DeviceModelNumber = "Sensor123",
            HomeId = Guid.NewGuid()
        };
        var secondHardware = new Hardware
        {
            DeviceModelNumber = "Sensor456",
            HomeId = Guid.NewGuid()
        };
        var hardwareIds = new List<Guid> { firstHardware.Id, secondHardware.Id };
        var expectedHardwares = new List<Hardware> { firstHardware, secondHardware };

        _hardwareRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.Is<Expression<Func<Hardware, bool>>>(expr =>
                expr.Compile()(firstHardware))))
            .Returns(firstHardware);
        _hardwareRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.Is<Expression<Func<Hardware, bool>>>(expr =>
                expr.Compile()(secondHardware))))
            .Returns(secondHardware);

        var result = _homeService.GetHardwaresByIds(hardwareIds);

        result.Should().BeEquivalentTo(expectedHardwares);
    }

    [TestMethod]
    public void ValidateHardwaresBelongToHome_WhenHardwareIdsDontBelongToHome_ShouldThrow()
    {
        var homeId = Guid.NewGuid();
        var hardwareIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var expectedHardwares = hardwareIds.Select(id => new Hardware
        {
            DeviceModelNumber = "Sensor123",
            HomeId = Guid.NewGuid()
        }).ToList();

        _hardwareRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns<Expression<Func<Hardware, bool>>>(expr =>
                expectedHardwares.FirstOrDefault(expr.Compile()));

        var act = () => _homeService.ValidateHardwaresBelongToHome(homeId, hardwareIds);

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenRoomAndHardwareExist_ShouldAddHardwareToRoom()
    {
        var home = GetValidHome();
        var homeId = home.Id;
        var room = new Room { Name = "Living room", HomeId = homeId, Home = home, Hardwares = [] };
        home.Rooms.Add(room);
        var hardware = new Hardware { DeviceModelNumber = "Sensor123", HomeId = room.HomeId };
        home.Hardwares.Add(hardware);
        var loggedUser = GetValidUser();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = homeId };

        _homeRepositoryMock
            .Setup(hr => hr.GetRoom(room.Id))
            .Returns(room);
        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _homeRepositoryMock
            .Setup(hr => hr.SetHardwareRoom(room, hardware));

        var act = () => _homeService.SetHardwareRoom(room.Id, hardware.Id, loggedUser);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenRoomDoesntExist_ShouldThrow()
    {
        var homeId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var loggedUser = GetValidUser();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = homeId };

        _homeRepositoryMock
            .Setup(hr => hr.GetRoom(roomId))
            .Returns((Room)null);

        var act = () => _homeService.SetHardwareRoom(roomId, hardwareId, loggedUser);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenHardwareDoesntExist_ShouldThrow()
    {
        var homeId = Guid.NewGuid();
        var room = new Room { Name = "Living room", HomeId = homeId, Hardwares = [] };
        var hardwareId = Guid.NewGuid();
        var loggedUser = GetValidUser();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = homeId };

        _homeRepositoryMock
            .Setup(hr => hr.GetRoom(room.Id))
            .Returns(room);
        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns((Hardware)null);

        var act = () => _homeService.SetHardwareRoom(room.Id, hardwareId, loggedUser);
        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenHardwareDoesntBelongToHome_ShouldThrow()
    {
        var home = GetValidHome();
        var room = new Room { Name = "Living room", HomeId = home.Id, Home = home, Hardwares = [] };
        var hardware = new Hardware { DeviceModelNumber = "Sensor123", HomeId = Guid.NewGuid() };
        var loggedUser = GetValidUser();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = home.Id };

        _homeRepositoryMock
            .Setup(hr => hr.GetRoom(room.Id))
            .Returns(room);
        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);

        var act = () => _homeService.SetHardwareRoom(room.Id, hardware.Id, loggedUser);

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void SetHardwareRoom_WhenHardwareAlreadyInRoom_ShouldThrow()
    {
        var home = GetValidHome();
        var homeId = home.Id;
        var room = new Room { Name = "Living room", HomeId = homeId, Home = home, Hardwares = [] };
        var hardware = new Hardware { DeviceModelNumber = "Sensor123", HomeId = room.HomeId };
        home.Rooms.Add(room);
        room.Hardwares.Add(hardware);
        var loggedUser = GetValidUser();
        var member = new Member { UserEmail = loggedUser.Email, HomeId = homeId };

        _homeRepositoryMock
            .Setup(hr => hr.GetRoom(room.Id))
            .Returns(room);
        _hardwareRepositoryMock
            .Setup(hr => hr.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);

        var act = () => _homeService.SetHardwareRoom(room.Id, hardware.Id, loggedUser);

        act.Should().Throw<InvalidOperationException>().WithMessage("Hardware already belongs to room");
    }

    [TestMethod]
    public void HasHomePermission_WhenMemberExistsAndHasPermission_ShouldReturnTrue()
    {
        var member = new Member { UserEmail = "seba@gmail.com", HomeId = Guid.NewGuid(), HomePermissions = [] };
        var permission = new HomePermission { Name = "AddDevice", MemberId = member.Id };
        member.HomePermissions.Add(permission);

        _memberRepositoryMock
            .Setup(m => m.GetOrDefault(It.IsAny<Expression<Func<Member, bool>>>()))
            .Returns(member);

        var act = _homeService.HasHomePermission(member.UserEmail, "AddDevice");

        act.Should().BeTrue();
    }

    [TestMethod]
    public void HasHomePermission_WhenMemberDoesntExist_ShouldThrow()
    {
        _memberRepositoryMock
            .Setup(m => m.GetOrDefault(It.IsAny<Expression<Func<Member, bool>>>()))
            .Returns((Member)null);

        var act = () => _homeService.HasHomePermission(string.Empty, "AddDevice");

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenMemberEmailIsNull_ShouldThrowArgumentNullException()
    {
        var home = GetValidHome();
        var user = home.Owner;

        var act = () => _homeService.AddPermissionToMember(home.Id, null, "AddDevice");

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenHomePermissionNameIsNull_ShouldThrowArgumentNullException()
    {
        var home = GetValidHome();
        var user = home.Owner;

        var act = () => _homeService.AddPermissionToMember(home.Id, user.Email, null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenHomeDoesNotExist_ShouldThrowNotFoundException()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var homeId = Guid.NewGuid();

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(false);

        var act = () => _homeService.AddPermissionToMember(homeId, user.Email, "AddDevice");

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenMemberDoesNotExist_ShouldThrowNotFoundException()
    {
        var home = GetValidHome();
        var user = home.Owner;

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(null as Member);

        var act = () => _homeService.AddPermissionToMember(home.Id, user.Email, "AddDevice");

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenHomePermissionDoesNotExist_ShouldThrowNotFoundException()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var member = new Member { User = user, UserEmail = user.Email, HomeId = home.Id };

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);

        var act = () => _homeService.AddPermissionToMember(home.Id, member.UserEmail, "InvalidPermission");

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddPermissionToMember_WhenMemberAlreadyHasPermission_ShouldThrowInvalidOperationException()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var member = new Member { UserEmail = user.Email, HomeId = home.Id, HomePermissions = [new HomePermission { Name = "AddDevice", MemberId = Guid.NewGuid() }] };

        _homeRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>())).Returns(true);
        _homeRepositoryMock.Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);

        var act = () => _homeService.AddPermissionToMember(home.Id, member.UserEmail, "AddDevice");

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void SetHardwareName_WhenOk_ShouldSetHardwareName()
    {
        var hardware = new Hardware { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };
        const string name = "Polycom";

        _homeRepositoryMock
            .Setup(hr => hr.SetHardwareName(hardware.Id, name))
            .Callback<Guid, string>((h, n) =>
            {
                h.Should().Be(hardware.Id);
                n.Should().Be(name);
                hardware.Name = name;
            });

        _homeService.SetHardwareName(hardware.Id, name);

        hardware.Name.Should().Be(name);
    }

    [TestMethod]
    public void SetHardwareName_WhenNameIsNull_ShouldThrow()
    {
        var hardware = new Hardware { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };
        const string name = null;

        var act = () => _homeService.SetHardwareName(hardware.Id, name);

        act.Should().Throw<ArgumentNullException>();
    }

    public void SetHomeName_WhenHomeExists_ShouldSetHomeName()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var newName = "New Home";

        _homeRepositoryMock
            .Setup(hr => hr.SetHomeName(It.IsAny<Guid>(), It.IsAny<string>()))
            .Callback<Guid, string>((id, name) =>
            {
                id.Should().Be(home.Id);
                name.Should().Be(newName);
                home.Name = name;
            });

        _homeService.SetHomeName(home.Id, newName);

        home.Name.Should().Be(newName);
    }

    [TestMethod]
    public void AddHardware_WhenAddingLampAndInfoIsCorrect_ShouldAddDeviceLamp()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var device = new Device()
        {
            Name = "Lamp",
            ModelNumber = "111AAA",
            Description = "White",
            Photos = ["photo1", "photo2"],
            CompanyRUT = GetValidCompany().RUT,
            DeviceTypeName = ValidDeviceTypes.Lamp.ToString()
        };

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(device);
        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _lampHardwareRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<LampHardware>()));

        _homeService.AddHardware(home.Id, device.ModelNumber);
    }

    [TestMethod]
    public void AddHardware_WhenAddingMovementSensorAndInfoIsCorrect_ShouldAddDeviceMovementSensor()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var device = new Device()
        {
            Name = "Movement Sensor",
            ModelNumber = "111AAA",
            Description = "White",
            Photos = ["photo1", "photo2"],
            CompanyRUT = GetValidCompany().RUT,
            DeviceTypeName = ValidDeviceTypes.MovementSensor.ToString()
        };

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(device);
        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(repo => repo.AddHardware(home.Id, It.IsAny<Hardware>()));

        _homeService.AddHardware(home.Id, device.ModelNumber);
    }

    [DataTestMethod]
    public void AddHardware_WhenArgumentsAreInvalid_ShouldThrow()
    {
        var act = () => _homeService.AddHardware(Guid.NewGuid(), null);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddHardware_WhenDeviceDoesNotExists_ShouldThrow()
    {
        var modelNumber = "1";

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns((Device)null);

        var act = () => _homeService.AddHardware(Guid.NewGuid(), modelNumber);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void AddHardware_WhenHomeDoesNotExists_ShouldThrow()
    {
        var modelNumber = "1";

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(GetValidDevice());
        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(false);

        var act = () => _homeService.AddHardware(Guid.NewGuid(), modelNumber);

        act.Should().Throw<NotFoundException>();
    }

    [TestMethod]
    public void GetHardwareData_WhenHardwareNotFound_ShouldThrowNotFoundException()
    {
        var hardwareId = Guid.NewGuid();
        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>())).Returns((Hardware)null);

        Action act = () => _homeService.GetHardwareData(hardwareId);

        act.Should().Throw<NotFoundException>().WithMessage("Hardware not found");
    }

    [TestMethod]
    public void GetHardwareData_WhenDeviceNotFound_ShouldThrowNotFoundException()
    {
        var home = GetValidHome();
        var hardware = new Hardware { DeviceModelNumber = "123", HomeId = home.Id };
        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>())).Returns(hardware);
        _deviceRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>())).Returns((Device)null);

        Action act = () => _homeService.GetHardwareData(hardware.Id);

        act.Should().Throw<NotFoundException>().WithMessage("Device not found");
    }

    [TestMethod]
    public void GetHardwareData_WhenLampHardwareFound_ShouldReturnHardwareData()
    {
        var home = GetValidHome();
        var hardware = new Hardware { DeviceModelNumber = "123", HomeId = home.Id };
        var device = new Device { Name = "Lampara", Description = "Una lampara", CompanyRUT = "111222333344", ModelNumber = "123", DeviceTypeName = nameof(ValidDeviceTypes.Lamp), Photos = ["photo1"] };
        var lamp = new LampHardware { DeviceModelNumber = "123", Name = "Lamp1", IsOn = true, Connected = true, HomeId = home.Id };

        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>())).Returns(hardware);
        _deviceRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>())).Returns(device);
        _lampHardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<LampHardware, bool>>>())).Returns(lamp);

        var result = _homeService.GetHardwareData(hardware.Id);

        result.Name.Should().Be(lamp.Name);
        result.ModelNumber.Should().Be(lamp.DeviceModelNumber);
        result.LampIsOn.Should().Be(lamp.IsOn);
        result.ConnectionStatus.Should().Be(lamp.Connected);
        result.MainPhoto.Should().Be(device.Photos.First());
        result.DoorSensorIsOpen.Should().BeFalse();
    }

    [TestMethod]
    public void GetHardwareData_WhenSensorHardwareFound_ShouldReturnHardwareData()
    {
        var home = GetValidHome();
        var hardware = new Hardware { DeviceModelNumber = "123", HomeId = home.Id };
        var device = new Device { Name = "Sensor", Description = "A sensor", CompanyRUT = "111222333344", ModelNumber = "123", DeviceTypeName = nameof(ValidDeviceTypes.Sensor), Photos = ["photo1"] };
        var sensor = new SensorHardware { DeviceModelNumber = "123", Name = "Sensor1", IsOpen = true, Connected = true, HomeId = home.Id };

        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>())).Returns(hardware);
        _deviceRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>())).Returns(device);
        _sensorHardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<SensorHardware, bool>>>())).Returns(sensor);

        var result = _homeService.GetHardwareData(hardware.Id);

        result.Name.Should().Be(sensor.Name);
        result.ModelNumber.Should().Be(sensor.DeviceModelNumber);
        result.LampIsOn.Should().BeFalse();
        result.ConnectionStatus.Should().Be(sensor.Connected);
        result.MainPhoto.Should().Be(device.Photos.First());
        result.DoorSensorIsOpen.Should().Be(sensor.IsOpen);
    }

    [TestMethod]
    public void GetHardwareData_WhenGeneralHardwareFound_ShouldReturnHardwareData()
    {
        var home = GetValidHome();
        var hardware = new Hardware { DeviceModelNumber = "123", HomeId = home.Id };
        var device = new Device { Name = "Lamp1", Description = "Una lampara", CompanyRUT = "111222333344", ModelNumber = "123", DeviceTypeName = nameof(ValidDeviceTypes.Lamp), Photos = ["photo1"] };
        var lamp = new LampHardware { DeviceModelNumber = "123", Name = "Lamp1", IsOn = false, Connected = true, HomeId = home.Id };

        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>())).Returns(hardware);
        _deviceRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>())).Returns(device);
        _lampHardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<LampHardware, bool>>>()))!.Returns(lamp);

        var result = _homeService.GetHardwareData(hardware.Id);

        result.Name.Should().Be(device.Name);
        result.ModelNumber.Should().Be(hardware.DeviceModelNumber);
        result.LampIsOn.Should().BeFalse();
        result.ConnectionStatus.Should().Be(hardware.Connected);
        result.MainPhoto.Should().Be(device.Photos.First());
        result.DoorSensorIsOpen.Should().BeFalse();
    }

    [TestMethod]
    public void GetHardwaresAsHardwareData_WhenHomeIdExists_ShouldReturnHardwareDataList()
    {
        var home = GetValidHome();
        var hardwares = new List<Hardware>
        {
            new Hardware { Name = "Lamp1", DeviceModelNumber = "1", HomeId = home.Id },
            new Hardware { Name = "Sensor", DeviceModelNumber = "2", HomeId = home.Id }
        };
        var devices = new List<Device>
        {
            new Device { Name = "Lamp1", Description = "Una lampara", CompanyRUT = "111222333344", ModelNumber = "1", DeviceTypeName = nameof(ValidDeviceTypes.Camera), Photos = ["photo1"] },
            new Device { Name = "Sensor1", Description = "Un sensor", CompanyRUT = "111222333344", ModelNumber = "2", DeviceTypeName = nameof(ValidDeviceTypes.Camera), Photos = ["photo2"] }
        };

        _homeRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>())).Returns(true);
        _homeRepositoryMock.Setup(repo => repo.GetHardwares(home.Id, null)).Returns(hardwares);
        _hardwareRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns((Expression<Func<Hardware, bool>> expr) => hardwares.FirstOrDefault(h => expr.Compile().Invoke(h)));
        _deviceRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns((Expression<Func<Device, bool>> expr) => devices.FirstOrDefault(d => expr.Compile().Invoke(d)));

        var result = _homeService.GetHardwaresAsHardwareData(home.Id);

        result.Should().HaveCount(2);
        result.First().Name.Should().Be(devices.First().Name);
        result.First().ModelNumber.Should().Be(hardwares.First().DeviceModelNumber);
        result.First().LampIsOn.Should().BeFalse();
        result.First().ConnectionStatus.Should().Be(hardwares.First().Connected);
        result.First().MainPhoto.Should().Be(devices.First().Photos!.First());
        result.First().DoorSensorIsOpen.Should().BeFalse();
    }

    [TestMethod]
    public void AddPermissionsToMember_WhenCalled_ShouldAddPermissions()
    {
        var home = GetValidHome();
        var user = home.Owner;
        var member = new Member { User = user, UserEmail = user.Email, HomeId = home.Id, HomePermissions = [] };
        var memberId = member.Id;
        var permissionName = ValidHomePermissions.AddDevice.ToString();
        var newPermission = new HomePermission
        {
            Name = permissionName,
            MemberId = member.Id
        };

        _homeRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(true);
        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(member);
        _homeRepositoryMock
            .Setup(repo => repo.AddPermissionToMember(It.IsAny<Guid>(), It.IsAny<HomePermission>()));

        var act = () => _homeService.AddPermissionsToMember(home.Id, member.UserEmail, ["AddDevice"]);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void GetMemberPermissions_WhenMemberExists_ShouldReturnPermissions()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";
        var member = new Member { UserEmail = userEmail, HomeId = homeId };
        var permissions = new List<HomePermission>
        {
            new HomePermission { MemberId = member.Id, Name = "Permission1" },
            new HomePermission { MemberId = member.Id, Name = "Permission2" }
        };

        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(homeId, userEmail))
            .Returns(member);
        _homeRepositoryMock
            .Setup(repo => repo.GetMemberPermissions(member.Id))
            .Returns(permissions);

        var result = _homeService.GetMemberPermissions(homeId, userEmail);

        result.Should().BeEquivalentTo(permissions);
    }

    [TestMethod]
    public void GetMemberPermissions_WhenMemberDoesNotExist_ShouldThrowNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "test@example.com";

        _homeRepositoryMock
            .Setup(repo => repo.GetOrDefaultMemberByHomeAndEmail(homeId, userEmail))
            .Returns((Member)null);

        Action act = () => _homeService.GetMemberPermissions(homeId, userEmail);

        act.Should().Throw<NotFoundException>().WithMessage("Member not found");
    }

    [TestMethod]
    public void UserIsTheHomeOwner_WhenHomeExistsAndUserIsOwner_ShouldReturnTrue()
    {
        var homeId = Guid.NewGuid();
        var user = GetValidUser();
        var home = GetValidHome();

        _homeRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);

        var result = _homeService.UserIsTheHomeOwner(homeId, user.Email!);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void UserIsTheHomeOwner_WhenHomeDoesNotExist_ShouldThrowNotFoundException()
    {
        var homeId = Guid.NewGuid();
        var userEmail = "user@example.com";

        _homeRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Home, bool>>>())).Returns((Home)null);

        Action act = () => _homeService.UserIsTheHomeOwner(homeId, userEmail);

        act.Should().Throw<NotFoundException>().WithMessage("Home not found");
    }
    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }

    private Company GetValidCompany()
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = GetValidUser().Email
        };
    }

    private Device GetValidDevice()
    {
        return new Device()
        {
            Name = "Polycom",
            ModelNumber = "1",
            Description = "Polycom device",
            Photos = ["photo1", "photo2"],
            CompanyRUT = GetValidCompany().RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
    }

    private Home GetValidHome()
    {
        var user = GetValidUser();

        return new Home
        {
            Owner = user,
            OwnerEmail = user.Email,
            Location = new Location("Golden street", "818"),
            Coordinates = new Coordinates("123", "456"),
            MemberCount = 4
        };
    }
    #endregion
}
