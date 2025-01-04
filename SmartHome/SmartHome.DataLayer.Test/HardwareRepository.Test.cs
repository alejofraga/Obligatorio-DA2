using FluentAssertions;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class HardwareRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private HardwareRepository _hardwareRepository = null!;
    private UserRepository _userRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _hardwareRepository = new HardwareRepository(_context);
        _userRepository = new UserRepository(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetOrDefault_WhenHardwareExists_ShouldGetHardware()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var company = GetValidCompany(user);
        _context.Companies.Add(company);
        _context.SaveChanges();
        var device = GetValidDevice(user);
        _context.Devices.Add(device);
        _context.SaveChanges();
        var home = GetValidHome(user, 1);
        _context.Homes.Add(home);
        _context.SaveChanges();
        var hardware = GetValidHardware(device, home.Id);
        _hardwareRepository.Add(hardware);

        var hardwares = _hardwareRepository.GetOrDefault(h => h.Id == hardware.Id);

        hardwares.Should().NotBeNull();
        hardwares.Should().BeEquivalentTo(hardware);
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456",
            ProfilePicturePath = "pathMati"
        };
    }

    private Company GetValidCompany(User user)
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = user.Email
        };
    }

    private Hardware GetValidHardware(Device device, Guid homeId)
    {
        return new Hardware()
        {
            DeviceModelNumber = device.ModelNumber,
            HomeId = homeId,
            Device = device,
            Connected = true,
        };
    }

    private Device GetValidDevice(User user)
    {
        return new Device()
        {
            ModelNumber = "5",
            CompanyRUT = GetValidCompany(user).RUT,
            Name = "Device",
            Description = "Description",
            Photos = ["www.photo.com"],
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };
    }

    private Home GetValidHome(User user, int memberCount)
    {
        return new Home()
        {
            Owner = user,
            Coordinates = new Coordinates()
            {
                Longitude = "13",
                Latitude = "12"
            },
            Location = new Location()
            {
                Address = "address",
                DoorNumber = "818"
            },
            OwnerEmail = user.Email,
            MemberCount = memberCount,
        };
    }
    #endregion
}
