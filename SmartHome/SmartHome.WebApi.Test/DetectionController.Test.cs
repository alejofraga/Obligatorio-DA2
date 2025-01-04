using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Test;

[TestClass]
public class CameraDetectionController_Test
{
    private static Mock<IHomeService> _homeServiceMock = null!;
    private CameraDetectionController _cameraDetectionController = null!;
    private static Mock<IUserService> _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);

    [TestInitialize]
    public void OnInitialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _cameraDetectionController = new CameraDetectionController(_homeServiceMock.Object, _userServiceMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeServiceMock.VerifyAll();
        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetCameraDetection_WhenOk_ShouldReturnOkResponse()
    {
        var hardwareId = Guid.NewGuid();

        _homeServiceMock
            .Setup(hs => hs.ExistHardwareOrThrow(hardwareId));
        _homeServiceMock
            .Setup(hs => hs.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        _homeServiceMock
            .Setup(hs => hs.SendNotification(hardwareId, "Movement detected!"));
        _homeServiceMock
            .Setup(hs => hs.AssertCameraHasMovementDetectionFeature(hardwareId));
        _homeServiceMock
            .Setup(hs => hs.AssertHardwareIsConnected(hardwareId));

        var controllerResponse = _cameraDetectionController.GenerateMovementDetectionNotification(hardwareId);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
    }

    [TestMethod]
    public void GenerateCameraPersonDetectionNotificationRequest_WhenOk_ShouldReturnOkResponse()
    {
        var hardwareId = Guid.NewGuid();
        var request = new GenerateCameraPersonDetectionNotificationRequest { IdentifiedUserEmail = "alejofraga22v2@gmail.com" };

        _homeServiceMock
            .Setup(hs => hs.ExistHardwareOrThrow(hardwareId));
        _homeServiceMock
            .Setup(hs => hs.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        _userServiceMock
            .Setup(us => us.GetByEmailOrThrow(request.IdentifiedUserEmail, "IdentifiedUserEmail")).Returns(GetValidUser());
        _homeServiceMock
            .Setup(hs => hs.SendNotification(hardwareId, $"Person detected! User identified: " +
                $"{GetValidUser().Name} {GetValidUser().Lastname} - {GetValidUser().Email}"));
        _homeServiceMock
            .Setup(hs => hs.AssertCameraHasPersonDetectionFeature(hardwareId));
        _homeServiceMock
            .Setup(hs => hs.AssertHardwareIsConnected(hardwareId));

        var controllerResponse = _cameraDetectionController.GeneratePersonDetectionNotification(hardwareId, request);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
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
    #endregion
}
