using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeController_Test
{
    private static Mock<IHomeService> _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
    private HomeController _homeController = new HomeController(_homeServiceMock.Object);
    private Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _homeController = new HomeController(_homeServiceMock.Object);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void AddMember_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var homeOwner = GetValidUser();
        var home = GetValidHome();
        var addMemberRequest = new AddMemberRequest() { UserEmail = "maticor93@gmail.com" };
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.AssertUserLoggedIsHomeOwner(It.IsAny<Guid>(), It.IsAny<User>()));
        _homeServiceMock
            .Setup(hs => hs.AddMember(It.IsAny<Guid>(), It.IsAny<string>()));
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(homeOwner);

        var controllerResponse = _homeController.AddMember(home.Id, addMemberRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().Be("Member added to home successfully");
    }

    [TestMethod]
    public void GetMembers_WhenInfoIsOk_ShouldReturnMembers()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var member = GetValidMember(home, user);

        _homeServiceMock
            .Setup(hs => hs.GetMembers(It.IsAny<Guid>()))
            .Returns([member]);

        var act = _homeController.GetMembers(home.Id);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<GetMembersResponse>;
        responseList.Should().NotBeNull();
        var memberResponse = responseList!.First();
        memberResponse.Should().NotBeNull();
        memberResponse.Name.Should().Be(user.Name);
        memberResponse.Lastname.Should().Be(user.Lastname);
        memberResponse.Email.Should().Be(user.Email);
        memberResponse.ProfilePicturePath.Should().Be(user.ProfilePicturePath);
        memberResponse.HomePermissions.Should()
            .BeEquivalentTo(member.HomePermissions.Select(permission => permission.Name).ToList());
        memberResponse.ReceiveNotifications.Should().BeFalse();
    }

    [TestMethod]
    public void GetHardwares_WhenInfoIsOk_ShouldReturnHardwares()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var device = GetValidDevice();
        var hardware = GetValidHardware(home, device);
        var hardwaresData = new List<HardwareData>
        {
            new HardwareData
            {
                Name = device.Name!,
                ModelNumber = device.ModelNumber!,
                MainPhoto = device.Photos!.First(),
                ConnectionStatus = true,
                LampIsOn = false,
                DoorSensorIsOpen = false
            }
        };
        var getHardwaresFilterRequest = new GetHardwaresFilterRequest() { RoomName = "Living Room" };

        _homeServiceMock
            .Setup(hs => hs.GetHardwaresAsHardwareData(It.IsAny<Guid>(), getHardwaresFilterRequest.RoomName))
            .Returns(hardwaresData);

        var act = _homeController.GetHardwares(home.Id, getHardwaresFilterRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<HardwareData>;
        responseList.Should().NotBeNull();
        var hardwareResponse = responseList!.First();
        hardwareResponse.Should().NotBeNull();
        hardwareResponse.Name.Should().Be(device.Name);
        hardwareResponse.ModelNumber.Should().Be(device.ModelNumber);
        hardwareResponse.MainPhoto.Should().Be(device.Photos!.First());
        hardwareResponse.ConnectionStatus.Should().BeTrue();
    }

    [TestMethod]
    public void AddHardware_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var device = GetValidDevice();
        var addHardwareRequest = new AddHardwareRequest() { ModelNumber = device.ModelNumber };
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.AddHardware(It.IsAny<Guid>(), It.IsAny<string>()));

        var controllerResponse = _homeController.AddHardware(home.Id, addHardwareRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value).ToString();
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Device added to home successfully");
    }

    [TestMethod]
    public void GrantHomePermission_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var member = GetValidMember(home, user);
        var homePermission = new HomePermission() { Name = "receiveNotifications", MemberId = member.Id };
        var grantHomePermissionRequest = new GrantHomePermissionRequest()
        {
            MemberEmail = member.UserEmail,
            HomePermissions = [homePermission.Name]
        };
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.AddPermissionsToMember(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()));

        var controllerResponse =
            _homeController.GrantHomePermission(member.HomeId, grantHomePermissionRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Home permission granted successfully");
        _homeServiceMock.VerifyAll();
        homePermission.MemberId.Should().Be(member.Id);
    }

    [TestMethod]
    public void AddRoom_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var homeOwner = GetValidUser();
        var home = GetValidHome();
        var addRoomRequest = new AddRoomRequest() { Name = "Living Room", HardwareIds = [] };
        var newRoom = new Room() { Name = addRoomRequest.Name, HomeId = home.Id, Hardwares = [] };
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.AddRoom(It.IsAny<CreateRoomArgs>()))
            .Returns(newRoom);

        var act = _homeController.AddRoom(home.Id, addRoomRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = (CreateRoomResponse)result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.RoomName.Should().Be(newRoom.Name);
    }

    [TestMethod]
    public void GetHomeOwner_WhenCalled_ShouldReturnOwnerStatus()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var isOwner = true;
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.UserIsTheHomeOwner(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(isOwner);
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(user);

        var controllerResponse = _homeController.GetHomeOwner(home.Id);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().Be(isOwner);
    }

    [TestMethod]
    public void GetMemberPermissions_WhenCalled_ShouldReturnPermissions()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var member = GetValidMember(home, user);
        var permission = new HomePermission() { MemberId = member.Id, Name = "Permission1" };
        var permissions = new List<HomePermission>() { permission };
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.GetMemberPermissions(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(permissions);
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(user);

        var controllerResponse = _homeController.GetMemberPermissions(home.Id);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<string>;
        responseList.Should().NotBeNull();
        responseList.Should().BeEquivalentTo(permissions.Select(p => p.Name).ToList());
    }

    [TestMethod]
    public void GetRooms_WhenHomeHasRooms_ShouldGetRooms()
    {
        var home = GetValidHome();
        var args = new AddRoomRequest() { Name = "Living Room", HardwareIds = [] };
        var newRoom = new Room() { Name = args.Name, HomeId = home.Id, Hardwares = [] };

        List<Room> expectedRooms = [newRoom];
        _homeController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _homeServiceMock
            .Setup(hs => hs.GetRooms(home.Id))
            .Returns(expectedRooms);

        var act = _homeController.GetRooms(home.Id);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result?.Value?.GetType().GetProperty("Data")?.GetValue(result.Value) as GetRoomsResponse;
        responseValue.Should().NotBeNull();
        responseValue!.Rooms.Count.Should().Be(1);
    }

    #region SampleData

    private User GetValidUser()
    {
        return new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "#Adf123456"
        };
    }

    private Home GetValidHome()
    {
        return new Home()
        {
            OwnerEmail = "alejofraga22v2@gmail.com",
            Coordinates = new Coordinates("123", "456"),
            Location = new Location("Golden street", "818"),
            MemberCount = 1,
        };
    }

    private Member GetValidMember(Home home, User user)
    {
        var member = new Member() { Home = home, User = user, HomeId = home.Id, UserEmail = user.Email };

        home.Members.Add(member);

        return member;
    }

    private Device GetValidDevice()
    {
        var validatorType = "ValidatorLength";

        return new Device()
        {
            Name = "Sensor",
            ModelNumber = "1",
            Description = "Description",
            Photos = ["www.photo.com"],
            Company = new Company(validatorType)
            {
                Name = "Company",
                RUT = "123456789012",
                LogoUrl = "www.cameraSA.com",
                OwnerEmail = "owner@gmail.com"
            },
            CompanyRUT = "123456789012",
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
    }

    private Hardware GetValidHardware(Home home, Device device)
    {
        var hardware = new Hardware
        {
            DeviceModelNumber = device.ModelNumber,
            Connected = true,
            HomeId = home.Id,
            Device = device,
            Name = device.Name
        };

        home.Hardwares.Add(hardware);

        return hardware;
    }

    #endregion
}
