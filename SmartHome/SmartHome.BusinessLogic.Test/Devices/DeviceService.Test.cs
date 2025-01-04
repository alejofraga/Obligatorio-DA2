using System.Linq.Expressions;
using FluentAssertions;
using Importer;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Devices;

[TestClass]
public class DeviceService_Test
{
    private static Mock<IDeviceRepository> _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
    private static Mock<IModelValidator> _modelValidatorMock = new Mock<IModelValidator>(MockBehavior.Strict);
    private static Mock<IDeviceImporter> _deviceImporterMock = new Mock<IDeviceImporter>(MockBehavior.Strict);

    private static readonly Mock<ICompanyRepository> _companyRepositoryMock =
        new Mock<ICompanyRepository>(MockBehavior.Strict);

    private DeviceService _deviceService = new DeviceService(_deviceRepositoryMock.Object,
        _companyRepositoryMock.Object, _modelValidatorMock.Object, _deviceImporterMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _deviceRepositoryMock = new Mock<IDeviceRepository>(MockBehavior.Strict);
        _modelValidatorMock = new Mock<IModelValidator>(MockBehavior.Strict);
        _deviceImporterMock = new Mock<IDeviceImporter>(MockBehavior.Strict);
        _deviceService = new DeviceService(_deviceRepositoryMock.Object, _companyRepositoryMock.Object,
            _modelValidatorMock.Object, _deviceImporterMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _deviceRepositoryMock.VerifyAll();
        _modelValidatorMock.VerifyAll();
        _companyRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void AddDevice_WhenDeviceIsCorrect_ShouldAddDevice()
    {
        var args = new CreateBasicDeviceArgs()
        {
            ModelNumber = "AAAAAA",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
            DeviceTypeName = nameof(ValidDeviceTypes.Sensor)
        };

        _deviceRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(false);
        _companyRepositoryMock
            .Setup(cr => cr.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());
        _deviceRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Device>()));
        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var newDevice = _deviceService.AddDevice(args, GetValidUser());

        newDevice.Should().NotBeNull();
        newDevice.ModelNumber.Should().Be(args.ModelNumber);
        newDevice.Name.Should().Be(args.Name);
        newDevice.Description.Should().Be(args.Description);
        newDevice.Photos.Should().BeEquivalentTo(args.Photos);
    }

    [TestMethod]
    public void AddDevice_WhenDeviceModelIsInvalid_ShouldThrow()
    {
        var createBasicDeviceArgs = new CreateBasicDeviceArgs()
        {
            ModelNumber = "1",
            Name = "name",
            Description = "description",
            Photos = ["photo1"],
            DeviceTypeName = nameof(ValidDeviceTypes.Sensor)
        };

        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        _companyRepositoryMock
            .Setup(cr => cr.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());

        var act = () => _deviceService.AddDevice(createBasicDeviceArgs, GetValidUser());

        act.Should().Throw<ArgumentException>().WithMessage("Model number is not valid according to ValidatorLength");
    }

    [TestMethod]
    public void AddCamera_WhenCameraIsCorrect_ShouldAddCamera()
    {
        var args = new CreateCameraArgs()
        {
            ModelNumber = "AAAAAA",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            HasMovementDetection = true,
            HasPersonDetection = true,
            IsOutdoor = true,
            IsIndoor = true
        };

        _deviceRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(false);
        _companyRepositoryMock
            .Setup(cr => cr.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());
        _deviceRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Device>()));
        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var newCamera = _deviceService.AddCamera(args, GetValidUser());

        newCamera.Should().NotBeNull();
        newCamera.ModelNumber.Should().Be(args.ModelNumber);
        newCamera.Name.Should().Be(args.Name);
        newCamera.Description.Should().Be(args.Description);
        newCamera.Photos.Should().BeEquivalentTo(args.Photos);
        newCamera.HasMovementDetection.Should().Be(args.HasMovementDetection);
        newCamera.HasPersonDetection.Should().Be(args.HasPersonDetection);
        newCamera.IsOutdoor.Should().Be(args.IsOutdoor);
        newCamera.IsIndoor.Should().Be(args.IsIndoor);
    }

    [TestMethod]
    public void AddCamera_WhenCameraModelIsInvalid_ShouldThrow()
    {
        var createCameraArgs = new CreateCameraArgs()
        {
            ModelNumber = "1",
            Name = "name",
            Description = "description",
            Photos = ["photo1"],
            HasMovementDetection = true,
            HasPersonDetection = true,
            IsOutdoor = true,
            IsIndoor = true
        };

        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        _companyRepositoryMock
            .Setup(cr => cr.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());

        var act = () => _deviceService.AddCamera(createCameraArgs, GetValidUser());

        act.Should().Throw<ArgumentException>().WithMessage("Model number is not valid according to ValidatorLength");
    }

    [TestMethod]
    public void AddCamera_WhenCameraIsDuplicated_ShouldThrow()
    {
        var args = new CreateCameraArgs()
        {
            ModelNumber = "AAAAAA",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            HasMovementDetection = true,
            HasPersonDetection = true,
            IsOutdoor = true,
            IsIndoor = true
        };

        _deviceRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(true);
        _companyRepositoryMock
            .Setup(cr => cr.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());
        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var act = () => _deviceService.AddCamera(args, GetValidUser());

        act.Should().Throw<InvalidOperationException>().WithMessage("Model number already in use");
    }

    [TestMethod]
    public void AddCamera_WhenCompanyNotFound_ShouldThrow()
    {
        var args = new CreateCameraArgs()
        {
            ModelNumber = "AAAAAA",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            HasMovementDetection = true,
            HasPersonDetection = true,
            IsOutdoor = true,
            IsIndoor = true
        };

        _companyRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(null as Company);

        var act = () => _deviceService.AddCamera(args, GetValidUser());

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Get_WhenModelNumberIsCorrect_ShouldGetDevice()
    {
        const string expectedModelNumber = "1";
        var newDevice = new Device()
        {
            ModelNumber = expectedModelNumber,
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = GetValidCompany().RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };

        _deviceRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(newDevice);

        var result = _deviceService.Get(expectedModelNumber);

        result.Should().BeEquivalentTo(newDevice);
    }

    [TestMethod]
    public void Add_WhenModelNumberIsDuplicated_ShouldThrowArgumentException()
    {
        var duplicatedDeviceArgs = new CreateBasicDeviceArgs()
        {
            ModelNumber = "ABCDDD",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
            DeviceTypeName = "Sensor"
        };

        _companyRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(GetValidCompany());
        _deviceRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(true);
        _modelValidatorMock
            .Setup(mv => mv.IsValid(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var act = () => _deviceService.AddDevice(duplicatedDeviceArgs, GetValidUser());

        act.Should().Throw<InvalidOperationException>().WithMessage("Model number already in use");
    }

    [TestMethod]
    public void AddDevice_WhenCompanyNotFound_ShouldThrow()
    {
        var duplicatedDeviceArgs = new CreateBasicDeviceArgs()
        {
            ModelNumber = "ABCDDD",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
        };

        _companyRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>()))
            .Returns(null as Company);

        var act = () => _deviceService.AddDevice(duplicatedDeviceArgs, GetValidUser());

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenFiltersAreCorrect_ShouldReturnFilteredDevices()
    {
        var expectedCompany = GetValidCompany();
        var expectedDevice = new Device()
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = expectedCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
        var getDevicesArgs = new GetDevicesArgs()
        {
            CompanyName = expectedCompany.Name,
            DeviceName = expectedDevice.Name,
            DeviceType = expectedDevice.DeviceTypeName,
            ModelNumber = expectedDevice.ModelNumber,
            Offset = 0,
            Limit = 25
        };

        _deviceRepositoryMock
            .Setup(repo => repo.GetDevicesWithFilters(It.IsAny<GetDevicesArgs>()))
            .Returns([expectedDevice]);

        var result = _deviceService.GetDevicesWithFilters(getDevicesArgs);

        result.Should().BeEquivalentTo([expectedDevice]);
    }

    [TestMethod]
    public void GetImporterParams_ReturnsParams()
    {
        var importerName = "Importer1";
        var importerParams = new Dictionary<string, string> { { "Param1", "Value1" } };
        _deviceImporterMock.Setup(importer => importer.GetImporterParams(importerName)).Returns(importerParams);
        var result = _deviceService.GetImporterParams(importerName);
        Assert.AreEqual(importerParams, result);
    }

    [TestMethod]
    public void GetImporterNames_ReturnsNames()
    {
        var importerNames = new List<string> { "Importer1", "Importer2" };
        _deviceImporterMock.Setup(importer => importer.GetImporterNames()).Returns(importerNames);
        var result = _deviceService.GetImporterNames();
        Assert.AreEqual(importerNames, result);
    }

    [TestMethod]
    public void ImportDevices_ImportsDevicesSuccessfully()
    {
        var importerName = "Importer1";
        var parameters = new Dictionary<string, string> { { "Param1", "Value1" } };
        var user = GetValidUser();
        var deviceDtos = new List<DeviceDto>
        {
            new DeviceDto
            {
                DeviceType = "Camera",
                ModelNumber = "Model1",
                Name = "Camera1",
                Photos = ["photo1.jpg"],
                HasMovementDetection = true,
                HasPersonDetection = true
            },
            new DeviceDto
            {
                DeviceType = "Sensor",
                ModelNumber = "Model2",
                Name = "Device1",
                Photos = ["photo2.jpg"]
            }
        };
        _deviceImporterMock.Setup(importer => importer.Import(importerName, parameters, user)).Returns(deviceDtos);
        _companyRepositoryMock.Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<Company, bool>>>())).Returns(
            new Company
            {
                OwnerEmail = user.Email,
                ValidatorTypeName = "Validator",
                Name = "name",
                LogoUrl = "/.com",
                RUT = "111111111111"
            });
        _deviceRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _deviceRepositoryMock.Setup(repo => repo.Add(It.IsAny<Device>()));
        _deviceService.ImportDevices(importerName, parameters, user);
        _deviceRepositoryMock.Verify(repo => repo.Add(It.IsAny<Device>()), Times.Exactly(2));
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenFiltersAreIncorrect_ShouldReturnEmptyList()
    {
        var getDevicesArgs = new GetDevicesArgs()
        {
            CompanyName = "Wrong Company",
            DeviceName = "Wrong Device",
            DeviceType = "Wrong Type",
            ModelNumber = "Wrong Model",
            Offset = 0,
            Limit = 25
        };

        _deviceRepositoryMock
            .Setup(repo => repo.GetDevicesWithFilters(It.IsAny<GetDevicesArgs>()))
            .Returns([]);

        var result = _deviceService.GetDevicesWithFilters(getDevicesArgs);

        result.Should().BeEmpty();
    }

    #region SampleData

    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }

    private Company GetValidCompany()
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = GetValidUser().Email
        };
    }

    #endregion
}
