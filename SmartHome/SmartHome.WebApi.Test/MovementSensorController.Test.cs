using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;
[TestClass]
public class MovementSensorController_Test
{
    private static Mock<IDeviceService> _deviceService = null!;
    private MovementSensorController _movementSensorController = null!;
    private static Mock<HttpContext> _httpContext = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _deviceService = new Mock<IDeviceService>(MockBehavior.Strict);
        _httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        _movementSensorController = new MovementSensorController(_deviceService.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _deviceService.VerifyAll();
        _httpContext.VerifyAll();
    }

    [TestMethod]
    public void CreateMovementSensor_WhenRequestIsValid_ShouldReturnOk()
    {
        var createMovementSensorRequest = new CreateBasicDeviceRequest()
        {
            Name = "name",
            Description = "description",
            ModelNumber = "modelNumber",
            Photos = ["photo1.jpg"],
        };
        var newDevice = new Device()
        {
            Name = createMovementSensorRequest.Name,
            Description = createMovementSensorRequest.Description,
            ModelNumber = createMovementSensorRequest.ModelNumber,
            Photos = ["photo1.jpg"],
            CompanyRUT = "123456789012",
            DeviceTypeName = nameof(ValidDeviceTypes.MovementSensor)
        };
        var userEmail = "user@example.com";
        var userLogged = new User
        {
            Email = "alejofraga22v2@gmail.com",
            Password = "123@4213Adsa",
            Name = "alejo",
            Lastname = "fraga"
        };
        _movementSensorController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext.Object
        };

        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(userLogged);
        _deviceService
            .Setup(d => d.AddDevice(It.IsAny<CreateBasicDeviceArgs>(), It.IsAny<User>()))
            .Returns(newDevice);

        var result = _movementSensorController.CreateMovementSensor(createMovementSensorRequest);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = (CreateDeviceResponse)objectResult.Value.GetType().GetProperty("Data")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.ModelNumber.Should().Be(newDevice.ModelNumber);
        responseValue.Name.Should().Be(newDevice.Name);
        responseValue.Description.Should().Be(newDevice.Description);
        responseValue.Photos.Should().BeEquivalentTo(newDevice.Photos);
        responseValue.DeviceType.Should().Be(newDevice.DeviceTypeName);
    }
}
