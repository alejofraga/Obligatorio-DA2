using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.DeviceTypes;

namespace SmartHome.BusinessLogic.Test.DeviceTypes;

[TestClass]
public class DeviceTypesService_Test
{
    [TestMethod]
    public void GetDeviceTypes_WhenCalled_ReturnsDeviceTypes()
    {
        var deviceTypeRepositoryMock = new Mock<IRepository<DeviceType>>(MockBehavior.Strict);
        var deviceTypesService = new DeviceTypesService(deviceTypeRepositoryMock.Object);
        var camera = new DeviceType() { Name = "camera" };
        var sensor = new DeviceType() { Name = "sensor" };
        var expectedDeviceTypes = new List<DeviceType>() { camera, sensor };

        deviceTypeRepositoryMock
            .Setup(x => x.GetAll(null))
            .Returns(expectedDeviceTypes);

        var deviceTypes = deviceTypesService.GetDeviceTypes();

        deviceTypes.Should().BeEquivalentTo(expectedDeviceTypes);
        deviceTypeRepositoryMock.VerifyAll();
    }
}
