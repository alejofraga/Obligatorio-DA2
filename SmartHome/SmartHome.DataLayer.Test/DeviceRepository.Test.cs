using FluentAssertions;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class DeviceRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private DeviceRepository _deviceRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _deviceRepository = new DeviceRepository(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenInfoIsCorrect_ShouldReturnFilteredDevicesWithOffsetAndLimit()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        var firstDevice = new Device()
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = firstCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Camera.ToString()
        };
        var secondDevice = new Device()
        {
            ModelNumber = "6",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
            CompanyRUT = secondCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
        _deviceRepository.Add(firstDevice);
        _deviceRepository.Add(secondDevice);
        var getDevicesArgs = new GetDevicesArgs()
        {
            Offset = 1,
            Limit = 1
        };

        var result = _deviceRepository.GetDevicesWithFilters(getDevicesArgs);

        result.Should().BeEquivalentTo([secondDevice]);
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenAllFiltersAreCorrect_ShouldReturnFilteredDevices()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        var firstDevice = new Device()
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = firstCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Camera.ToString()
        };
        var secondDevice = new Device()
        {
            ModelNumber = "6",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
            CompanyRUT = secondCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
        _deviceRepository.Add(firstDevice);
        _deviceRepository.Add(secondDevice);
        var getDevicesArgs = new GetDevicesArgs()
        {
            CompanyName = firstCompany.Name,
            DeviceName = firstDevice.Name,
            DeviceType = firstDevice.DeviceTypeName,
            ModelNumber = firstDevice.ModelNumber,
            Offset = 0,
            Limit = 5
        };

        var result = _deviceRepository.GetDevicesWithFilters(getDevicesArgs);

        result.Should().BeEquivalentTo([firstDevice]);
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenFiltersAreIncorrect_ShouldReturnEmptyList()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        var firstDevice = new Device()
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = firstCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Camera.ToString()
        };
        var secondDevice = new Device()
        {
            ModelNumber = "6",
            Name = "Sensor",
            Description = "This is a sensor",
            Photos = ["www.sensor.com"],
            CompanyRUT = secondCompany.RUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
        _deviceRepository.Add(firstDevice);
        _deviceRepository.Add(secondDevice);
        var getDevicesArgs = new GetDevicesArgs()
        {
            CompanyName = "Wrong company name",
            DeviceName = "Wrong device name",
            DeviceType = "Wrong device type",
            ModelNumber = "Wrong model number",
            Offset = 0,
            Limit = 5
        };

        var result = _deviceRepository.GetDevicesWithFilters(getDevicesArgs);

        result.Should().BeEmpty();
    }

    #region SampleData
    private User GetFirstValidUser()
    {
        var user = new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        _context.Add(user);

        return user;
    }

    private User GetSecondValidUser()
    {
        var user = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Fressi",
            Password = "#Adf123456"
        };
        _context.Add(user);

        return user;
    }

    private Company GetFirstValidCompany()
    {
        const string validatorType = "ValidatorLength";

        var company = new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = GetFirstValidUser().Email
        };
        _context.Add(company);

        return company;
    }

    private Company GetSecondValidCompany()
    {
        const string validatorType = "ValidatorLength";

        var company = new Company(validatorType)
        {
            Name = "SensorSA",
            RUT = "800450300728",
            LogoUrl = "www.sensorSA.com",
            OwnerEmail = GetSecondValidUser().Email
        };
        _context.Add(company);

        return company;
    }
    #endregion
}
