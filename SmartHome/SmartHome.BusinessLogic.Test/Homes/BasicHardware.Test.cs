using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Homes;

[TestClass]
public class BasicHardware_Test
{
    private static Mock<IHomeRepository> _homeRepositoryMock = null!;
    private static Mock<IUserRepository> _userRepositoryMock = null;
    private static Mock<IRepository<Coordinates>> _coordinatesRepositoryMock = null!;
    private static Mock<IRepository<Location>> _locationRepositoryMock = null!;
    private static Mock<IMemberRepository> _memberRepositoryMock = null;
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
        _lampHardwareRepositoryMock = new Mock<IRepository<LampHardware>>(MockBehavior.Strict);
        _cameraRepositoryMock = new Mock<IRepository<Camera>>(MockBehavior.Strict);
        _sensorHardwareRepositoryMock = new Mock<IRepository<SensorHardware>>(MockBehavior.Strict);

        _homeService = new HomeService(_homeRepositoryMock.Object, _userRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _coordinatesRepositoryMock.Object, _memberRepositoryMock.Object, _deviceRepositoryMock.Object,
            _hardwareRepositoryMock.Object, _cameraRepositoryMock.Object, _lampHardwareRepositoryMock.Object, _sensorHardwareRepositoryMock.Object);
    }

    [TestMethod]
    public void AssertCameraHasPersonDetectionFeature_WhenDoesNot_ShouldThrow()
    {
        var deviceModelNumber = "modelNumber";
        var homeId = Guid.NewGuid();
        var hardware = new Hardware() { DeviceModelNumber = deviceModelNumber, HomeId = homeId };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _cameraRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Camera, bool>>>()))
            .Returns(GetCameraWithoutPersonDetection());

        var act = () => _homeService.AssertCameraHasPersonDetectionFeature(homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera does not have person detection feature");
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertCameraHasPersonDetectionFeature_WhenDoes_ShouldNotThrow()
    {
        var deviceModelNumber = "modelNumber";
        var homeId = Guid.NewGuid();
        var hardware = new Hardware() { DeviceModelNumber = deviceModelNumber, HomeId = homeId };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _cameraRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Camera, bool>>>()))
            .Returns(GetCameraWithPersonDetection());

        var act = () => _homeService.AssertCameraHasPersonDetectionFeature(homeId);

        act.Should().NotThrow();
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertCameraHasMovementDetectionFeature_WhenDoesNot_ShouldThrow()
    {
        var deviceModelNumber = "modelNumber";
        var homeId = Guid.NewGuid();
        var hardware = new Hardware() { DeviceModelNumber = deviceModelNumber, HomeId = homeId };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _cameraRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Camera, bool>>>()))
            .Returns(GetCameraWithoutMovementDetection());

        var act = () => _homeService.AssertCameraHasMovementDetectionFeature(homeId);

        act.Should().Throw<InvalidOperationException>().WithMessage("Camera does not have movement detection feature");
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertCameraHasMovementDetectionFeature_WhenDoes_ShouldNotThrow()
    {
        var deviceModelNumber = "modelNumber";
        var homeId = Guid.NewGuid();
        var hardware = new Hardware() { DeviceModelNumber = deviceModelNumber, HomeId = homeId };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _cameraRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Camera, bool>>>()))
            .Returns(GetCameraWithMovementDetection());

        var act = () => _homeService.AssertCameraHasMovementDetectionFeature(homeId);

        act.Should().NotThrow();
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertCameraHasMovementDetectionFeature_WhenDeviceIsNull_ShouldNotThrow()
    {
        var deviceModelNumber = "modelNumber";
        var homeId = Guid.NewGuid();
        var hardware = new Hardware() { DeviceModelNumber = deviceModelNumber, HomeId = homeId };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(hardware);
        _cameraRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Camera, bool>>>()))
            .Returns(() => null);

        var act = () => _homeService.AssertCameraHasMovementDetectionFeature(homeId);

        act.Should().NotThrow();
        _hardwareRepositoryMock.VerifyAll();
        _cameraRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareStatus_WhenHardwareNotFound_ShouldThrow()
    {
        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(() => null);

        var act = () => _homeService.UpdateHardwareStatus(Guid.NewGuid(), true);

        act.Should().Throw<NotFoundException>().WithMessage("Hardware not found");
        _hardwareRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareStatus_WhenoK_ShouldUpdate()
    {
        var hardware = new Hardware() { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(() => hardware);
        _hardwareRepositoryMock
            .Setup(x => x.Update(It.IsAny<Hardware>()));

        _homeService.UpdateHardwareStatus(hardware.Id, true);

        _hardwareRepositoryMock.Verify(x => x.Update(It.IsAny<Hardware>()), Times.Once);
        _hardwareRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertHardwareIsConnected_WhenNot_ShouldThrow()
    {
        var hardware = new Hardware() { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };
        hardware.Connected = false;

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(() => hardware);

        var act = () => _homeService.AssertHardwareIsConnected(Guid.NewGuid());

        act.Should().Throw<InvalidOperationException>().WithMessage("Hardware is not connected");
        _hardwareRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AssertHardwareIsConnected_WhenItIs_ShouldNotThrow()
    {
        var hardware = new Hardware() { DeviceModelNumber = "1", HomeId = Guid.NewGuid() };
        hardware.Connected = true;

        _hardwareRepositoryMock
            .Setup(x => x.GetOrDefault(It.IsAny<Expression<Func<Hardware, bool>>>()))
            .Returns(() => hardware);

        var act = () => _homeService.AssertHardwareIsConnected(Guid.NewGuid());

        act.Should().NotThrow();
        _hardwareRepositoryMock.VerifyAll();
    }

    #region SampleData
    private Camera GetCameraWithoutPersonDetection()
    {
        return new Camera()
        {
            ModelNumber = "1",
            CompanyRUT = "12",
            HasMovementDetection = true,
            HasPersonDetection = false,
            Description = "Camera without person detection",
            Photos = ["sad"],
            Name = "camera",
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
    }

    private Camera GetCameraWithoutMovementDetection()
    {
        return new Camera()
        {
            ModelNumber = "1",
            CompanyRUT = "12",
            HasMovementDetection = false,
            HasPersonDetection = false,
            Description = "Camera without person detection",
            Photos = ["sad"],
            Name = "camera",
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
    }

    private Camera GetCameraWithMovementDetection()
    {
        return new Camera()
        {
            ModelNumber = "1",
            CompanyRUT = "12",
            HasMovementDetection = true,
            HasPersonDetection = false,
            Description = "Camera without person detection",
            Photos = ["sad"],
            Name = "camera",
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
    }

    private Camera GetCameraWithPersonDetection()
    {
        return new Camera()
        {
            ModelNumber = "1",
            CompanyRUT = "12",
            HasMovementDetection = true,
            HasPersonDetection = true,
            Description = "Camera without person detection",
            Photos = ["sad"],
            Name = "camera",
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };
    }
    #endregion
}
