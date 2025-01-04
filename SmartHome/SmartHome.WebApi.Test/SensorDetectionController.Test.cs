using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Homes;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApi.Test;
[TestClass]
public class SensorDetectionController_Test
{
    private SensorDetectionController sensorDetectionController = null!;
    private Mock<IHomeService> homeServiceMock = null!;
    [TestInitialize]
    public void TestInitialize()
    {
        homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        sensorDetectionController = new SensorDetectionController(homeServiceMock.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GenerateWindowMovementNotification_MovementDetectedIsOpened_ReturnsOk()
    {
        var hardwareId = Guid.NewGuid();

        homeServiceMock
            .Setup(h => h.ExistHardwareOrThrow(hardwareId));
        homeServiceMock
            .Setup(h => h.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        homeServiceMock
            .Setup(h => h.AssertHardwareIsConnected(hardwareId));
        homeServiceMock
            .Setup(h => h.SendNotificationIfSensorStateChanged(hardwareId, It.IsAny<string>(), true));

        var result = sensorDetectionController.GenerateWindowOpenedNotification(hardwareId);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
    }

    [TestMethod]
    public void GenerateWindowMovementNotification_MovementDetectedIsClosed_ReturnsOk()
    {
        var hardwareId = Guid.NewGuid();

        homeServiceMock
            .Setup(h => h.ExistHardwareOrThrow(hardwareId));
        homeServiceMock
            .Setup(h => h.AssertIsValidDevice(hardwareId, It.IsAny<string>()));
        homeServiceMock
            .Setup(h => h.AssertHardwareIsConnected(hardwareId));
        homeServiceMock
            .Setup(h => h.SendNotificationIfSensorStateChanged(hardwareId, It.IsAny<string>(), false));

        var result = sensorDetectionController.GenerateWindowClosedNotiication(hardwareId);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Members notified successfully!");
    }
}
