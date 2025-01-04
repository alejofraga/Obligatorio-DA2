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
public class CameraController_Test
{
    private static Mock<IDeviceService> _deviceService = null!;
    private CameraController _cameraController = null!;
    private static Mock<HttpContext> _httpContext = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _deviceService = new Mock<IDeviceService>(MockBehavior.Strict);
        _httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        _cameraController = new CameraController(_deviceService.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _deviceService.VerifyAll();
        _httpContext.VerifyAll();
    }

    [TestMethod]
    public void CreateCamera_WhenRequestIsValid_ShouldReturnOk()
    {
        var createCameraRequest = GetValidCreateCamaraRequest();
        var newCamera = GetValidCamera(createCameraRequest);
        var userEmail = "user@example.com";
        var userLogged = GetValidUser();
        _cameraController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext.Object
        };

        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(userLogged);
        _deviceService
            .Setup(d => d.AddCamera(It.IsAny<CreateCameraArgs>(), It.IsAny<User>()))
            .Returns(newCamera);

        var result = _cameraController.CreateCamera(createCameraRequest);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = (CreateCameraResponse)objectResult.Value.GetType().GetProperty("Data")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Name.Should().Be(newCamera.Name);
        responseValue.Description.Should().Be(newCamera.Description);
        responseValue.ModelNumber.Should().Be(newCamera.ModelNumber);
        responseValue.Photos.Should().BeEquivalentTo(newCamera.Photos);
        responseValue.HasMovementDetection.Should().BeTrue();
        responseValue.HasPersonDetection.Should().BeTrue();
        responseValue.IsIndoor.Should().BeFalse();
        responseValue.IsOutdoor.Should().BeTrue();
    }

    #region SampleData
    private CreateCameraRequest GetValidCreateCamaraRequest()
    {
        return new CreateCameraRequest
        {
            Name = "name",
            Description = "description",
            ModelNumber = "modelNumber",
            Photos = ["photo1.jpg"],
            HasMovementDetection = true,
            HasPersonDetection = true,
            IsOutdoor = true,
            IsIndoor = false
        };
    }

    private Camera GetValidCamera(CreateCameraRequest createCameraRequest)
    {
        return new Camera()
        {
            Name = createCameraRequest.Name,
            Description = createCameraRequest.Description,
            ModelNumber = createCameraRequest.ModelNumber,
            Photos = ["photo1.jpg"],
            HasMovementDetection = createCameraRequest.HasMovementDetection,
            HasPersonDetection = createCameraRequest.HasPersonDetection,
            IsOutdoor = createCameraRequest.IsOutdoor,
            IsIndoor = createCameraRequest.IsIndoor,
            CompanyRUT = "1234567892012",
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
    }

    private User GetValidUser()
    {
        return new User
        {
            Email = "alejofraga22v2@gmail.com",
            Password = "123@4213Adsa",
            Name = "alejo",
            Lastname = "fraga"
        };
    }
    #endregion
}
