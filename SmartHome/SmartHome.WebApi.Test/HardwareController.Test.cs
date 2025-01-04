using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;
[TestClass]
public class HardwareController_Test
{
    private static Mock<IHomeService> _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
    private HardwareController _hardwareController = new HardwareController(_homeServiceMock.Object);
    private Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _hardwareController = new HardwareController(_homeServiceMock.Object);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void ConnectHardware_WhenOk_ShouldConnectHardware()
    {
        var hardwareId = Guid.NewGuid();
        const bool connected = true;
        var connectedRequest = new UpdateHardwareStatusRequest { Connected = connected };

        _homeServiceMock
            .Setup(ds => ds.UpdateHardwareStatus(hardwareId, connected));

        var act = _hardwareController.UpdateHardwareStatus(hardwareId, connectedRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Hardware updated successfully");
    }

    [TestMethod]
    public void SetHardwareName_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var user = GetValidUser();
        var home = GetValidHome();
        var device = GetValidDevice();
        var hardware = GetValidHardware(home, device);
        var setHardwareNameRequest = new SetHardwareNameRequest() { Name = "NewName" };

        _homeServiceMock.Setup(hs => hs.SetHardwareName(It.IsAny<Guid>(), It.IsAny<string>()));

        var controllerResponse = _hardwareController.SetHardwareName(hardware.Id, setHardwareNameRequest);
        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Hardware name set successfully");
    }

    [TestMethod]
    public void SetHardwareRoom_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var addHardwareToRoomRequest = new SetHardwareRoomRequest { RoomId = Guid.NewGuid() };
        var hardwareId = Guid.NewGuid();
        var loggedUser = new User() { Name = "matias", Lastname = "corvetto", Email = "mati@cor.com", Password = "pa$s32Word" };
        var hardwareController = new HardwareController(_homeServiceMock.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
        };

        _homeServiceMock
            .Setup(hs => hs.SetHardwareRoom(addHardwareToRoomRequest.RoomId, hardwareId, loggedUser));
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(loggedUser);

        var controllerResponse = hardwareController.SetHardwareRoom(hardwareId, addHardwareToRoomRequest);

        var result = controllerResponse as ObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        Assert.IsNotNull(responseValue);
        Assert.AreEqual("Hardware added to room successfully", responseValue);
    }

    [TestMethod]
    public void SetHardwareRoom_WhenUserDoesNotHavePermission_ShouldThrowForbiddenAccessException()
    {
        var homeOwner = new User()
        {
            Name = "Seba",
            Lastname = "Vega",
            Email = "seba@vega.com",
            Password = "12A2$$#!ssasd"
        };
        var roomId = Guid.NewGuid();
        var addHardwareToRoomRequest = new SetHardwareRoomRequest { RoomId = Guid.NewGuid() };
        var hardwareController = new HardwareController(_homeServiceMock.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object }
        };

        _homeServiceMock
            .Setup(hs => hs.SetHardwareRoom(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<User>()))
            .Throws(new ForbiddenAccessException("User does not have permission to add hardware to room"));
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(homeOwner);

        Assert.ThrowsException<ForbiddenAccessException>(() => hardwareController.SetHardwareRoom(roomId, addHardwareToRoomRequest));
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
