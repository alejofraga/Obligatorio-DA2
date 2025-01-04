using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Homes;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApi.Test;

[TestClass]
public class LampDetectionController_Test
{
    private LampDetectionController _lampDetectionController = null!;
    private Mock<IHomeService> homeServiceMock = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _lampDetectionController = new LampDetectionController(homeServiceMock.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GenerateLampTurnedOnNotification_WhenLastStateIsTurnedOff_ReturnsOk()
    {
        var hardwareId = Guid.NewGuid();

        homeServiceMock
            .Setup(h => h.ExistHardwareOrThrow(hardwareId));
        homeServiceMock
            .Setup(h => h.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        homeServiceMock
            .Setup(h => h.AssertHardwareIsConnected(hardwareId));
        homeServiceMock
            .Setup(h => h.SendNotificationIfLampStateChanged(hardwareId, It.IsAny<string>(), true));

        var result = _lampDetectionController.GenerateTurnOnNotification(hardwareId);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
    }

    [TestMethod]
    public void GenerateLampTurnedOffNotification_WhenLastStateIsTurnedOff_ReturnsOk()
    {
        var hardwareId = Guid.NewGuid();

        homeServiceMock
            .Setup(h => h.ExistHardwareOrThrow(hardwareId));
        homeServiceMock
            .Setup(h => h.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        homeServiceMock
            .Setup(h => h.AssertHardwareIsConnected(hardwareId));
        homeServiceMock
            .Setup(h => h.SendNotificationIfLampStateChanged(hardwareId, It.IsAny<string>(), false));

        var result = _lampDetectionController.GenerateTurnOffNotification(hardwareId);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
    }
}
