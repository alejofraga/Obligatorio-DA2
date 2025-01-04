using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApi.Test;
[TestClass]
public class DeviceTypesController_Test
{
    private Mock<IDeviceTypesService> _deviceTypesServiceMock = null!;
    private DeviceTypesController _deviceTypesController = null!;
    [TestInitialize]
    public void TestInitialize()
    {
        _deviceTypesServiceMock = new Mock<IDeviceTypesService>();
        _deviceTypesController = new DeviceTypesController(_deviceTypesServiceMock.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _deviceTypesServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetDeviceTypes_WhenCalled_ReturnsDeviceTypes()
    {
        var camera = new DeviceType() { Name = "camera" };
        var sensor = new DeviceType() { Name = "sensor" };
        var deviceTypes = new List<DeviceType>() { camera, sensor };

        _deviceTypesServiceMock
            .Setup(x => x.GetDeviceTypes())
            .Returns(deviceTypes);

        var result = _deviceTypesController.GetDeviceTypes();

        var okResult = result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(deviceTypes);
    }
}
