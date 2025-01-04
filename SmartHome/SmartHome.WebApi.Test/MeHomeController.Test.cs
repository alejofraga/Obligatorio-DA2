using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;
[TestClass]
public class MeHomeController_Test
{
    private static Mock<IHomeService> _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
    private MeHomeController _meHomeController = new MeHomeController(_homeServiceMock.Object);
    private Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _meHomeController = new MeHomeController(_homeServiceMock.Object);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void CreateHome_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var createHomeRequest = new CreateHomeRequest()
        {
            Longitude = "123",
            Latitude = "123",
            Address = "Street",
            DoorNumber = "123",
            MemberCount = 1
        };
        var userLogged = new User()
        {
            Name = "Alejo",
            Lastname = "Fraga",
            Email = "alejofraga22v2@gmail.com",
            Password = "12A2$$#!ssasd"
        };
        var newHome = new Home()
        {
            OwnerEmail = userLogged.Email,
            Location = new Location("Street", "123"),
            Coordinates = new Coordinates("123", "123"),
            MemberCount = 1
        };
        var homeOwnerMember = new Member()
        {
            HomeId = newHome.Id,
            UserEmail = userLogged.Email
        };
        var expectedHomeId = Guid.NewGuid();
        _meHomeController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };

        _homeServiceMock
            .Setup(hs => hs.CreateHome(It.IsAny<CreateHomeArgs>()))
            .Returns(newHome);
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(userLogged);

        var controllerResponse = _meHomeController.CreateHome(createHomeRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();

        var responseValue = (CreateHomeResponse)result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.DoorNumber.Should().Be(newHome.Location.DoorNumber);
        responseValue.Address.Should().Be(newHome.Location.Address);
        responseValue.Latitude.Should().Be(newHome.Coordinates.Latitude);
        responseValue.Longitude.Should().Be(newHome.Coordinates.Longitude);
        responseValue.MemberCount.Should().Be(newHome.MemberCount);
        responseValue.Owner.Should().Be(newHome.OwnerEmail);
    }

    [TestMethod]
    public void GetHomesWithFilters_WhenRequestIsValid_ShouldReturnOkResponse()
    {
        var userLogged = new User()
        {
            Name = "Alejo",
            Lastname = "Fraga",
            Email = "alejofraga22v2@gmail.com",
            Password = "12A2$$#!ssasd"
        };
        var newHome = new Home()
        {
            Name = "HomeName",
            OwnerEmail = userLogged.Email,
            Location = new Location("Street", "123"),
            Coordinates = new Coordinates("123", "123"),
            MemberCount = 1
        };
        var expectedHomes = new List<Home>() { newHome };
        var filters = new GetHomesFilterRequest() { Limit = 3, Offset = 0 };
        _meHomeController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };

        _homeServiceMock
            .Setup(hs => hs.GetHomesWithFilters(It.IsAny<GetUserHomesArgs>()))
            .Returns(expectedHomes);
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(userLogged);

        var controllerResponse = _meHomeController.GetUserHomes(filters);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValues = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value) as List<GetUserHomesResponse>;
        var responseValue = (GetUserHomesResponse)responseValues?.First();
        responseValue.Should().NotBeNull();
        responseValue.Id.Should().Be(newHome.Id);
        responseValue.Name.Should().Be("HomeName");
    }
}
