using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;
[TestClass]
public class DeviceController_Test
{
    private static Mock<IDeviceService> _deviceServiceMock = new Mock<IDeviceService>(MockBehavior.Strict);
    private DeviceController _deviceController = new DeviceController(_deviceServiceMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>(MockBehavior.Strict);
        _deviceController = new DeviceController(_deviceServiceMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _deviceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetDevices_WhenFiltersAreCorrect_ShouldReturnDevicesFiltered()
    {
        var filters = new GetDevicesFiltersRequest()
        {
            CompanyName = "Company SA",
            Name = "Sensor1",
            DeviceType = "Sensor",
            ModelNumber = "154",
        };
        var user = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Password = "cdDS213123212313#4",
            Name = "Alejo",
            Lastname = "Fraga"
        };
        const string validatorType = "ValidatorLength";
        var expectedDevice = new Device()
        {
            Company = new Company(validatorType) { Name = filters.CompanyName, RUT = "111111111111", OwnerEmail = user.Email, LogoUrl = "www.cameraSA.com" },
            CompanyRUT = "111111111111",
            ModelNumber = filters.ModelNumber,
            Name = filters.Name,
            Description = "no description",
            Photos = ["Photo1", "Photo2"],
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };

        _deviceServiceMock
            .Setup(ds => ds.GetDevicesWithFilters(It.IsAny<GetDevicesArgs>()))
            .Returns([expectedDevice]);

        var act = _deviceController.GetDevices(filters);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<GetDevicesResponse>;
        responseList.Should().NotBeNull();
        const int expectedCount = 1;
        responseList.Count.Should().Be(expectedCount);
        responseList.First().CompanyName.Should().Be(expectedDevice.Company.Name);
        responseList.First().MainPhoto.Should().Be(expectedDevice.Photos.First());
        responseList.First().ModelNumber.Should().Be(expectedDevice.ModelNumber);
        responseList.First().Name.Should().Be(expectedDevice.Name);
    }
}
