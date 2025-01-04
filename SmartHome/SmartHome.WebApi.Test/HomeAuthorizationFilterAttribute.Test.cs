using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeAuthorizationFilterAttribute_Test
{
    private Mock<HttpContext> _httpContextMock = null!;
    private static Mock<IHomeService> _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
    private AuthorizationFilterContext _context = null!;
    private Mock<IServiceProvider> serviceProviderMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);

        serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IHomeService)))
            .Returns(_homeServiceMock.Object);

        _context = new AuthorizationFilterContext(
            new ActionContext(
                _httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor()),
            []);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _httpContextMock.VerifyAll();
        _homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void OnAuthorization_WhenHomeIdIsNull_ShouldReturnBadRequest()
    {
        var attribute = new HomeAuthorizationAttribute();
        var routeValues = new RouteValueDictionary();

        _httpContextMock
            .Setup(h => h.Request.RouteValues)
            .Returns(routeValues);

        attribute.OnAuthorization(_context);

        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetMessage(concreteResponse.Value!).Should().Be("Argument cannot be null or empty");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserIsNotAuthenticated_ShouldReturnUnauthorized()
    {
        var attribute = new HomeAuthorizationAttribute();
        var routeValues = new RouteValueDictionary
        {
            { "homeId", Guid.NewGuid().ToString() }
        };

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(null);
        _httpContextMock
            .Setup(hc => hc.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(h => h.Request.RouteValues)
            .Returns(routeValues);

        attribute.OnAuthorization(_context);

        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetMessage(concreteResponse.Value!).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenMemberHasNoHomePermission_ShouldReturnForbidden()
    {
        var userLogged = GetValidUser();
        var home = GetValidHome();
        var member = GetValidMember(home, userLogged);
        var routeValues = new RouteValueDictionary
        {
            { "homeId", home.Id.ToString() }
        };
        var permissionName = "addHardware";

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);
        _httpContextMock
            .Setup(hc => hc.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(hc => hc.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(h => h.Request.RouteValues)
            .Returns(routeValues);
        _homeServiceMock
            .Setup(h => h.GetOrDefaultMemberByHomeAndEmail(home.Id, userLogged.Email!))
            .Returns(member);

        var attribute = new HomeAuthorizationAttribute(permissionName);
        attribute.OnAuthorization(_context);
        _context.Result.Should().BeOfType<ObjectResult>();
        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetDetails(concreteResponse.Value!).Should().Be($"Missing permission {permissionName}");
    }

    [TestMethod]
    public void OnAuthorization_WhenMemberHasHomePermission_ShouldContinue()
    {
        var userLogged = GetValidUser();
        var home = GetValidHome();
        var permissionName = "addHardware";
        var member = GetValidMember(home, userLogged);
        AddValidHomePermission(permissionName, member);
        var routeValues = new RouteValueDictionary
        {
            { "homeId", home.Id.ToString() }
        };
        var attribute = new HomeAuthorizationAttribute(permissionName);

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);
        _httpContextMock
            .Setup(hc => hc.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(h => h.Request.RouteValues)
            .Returns(routeValues);
        _homeServiceMock
            .Setup(h => h.GetOrDefaultMemberByHomeAndEmail(home.Id, userLogged.Email!))
            .Returns(member);

        attribute.OnAuthorization(_context);

        _context.Result.Should().BeNull();
    }

    [TestMethod]
    public void OnAuthorization_WhenMemberHasWrongHomePermission_ShouldReturnForbidden()
    {
        var userLogged = GetValidUser();
        var home = GetValidHome();
        var permissionName = "addHardware";
        var otherPermissionName = "addMember";
        var member = GetValidMember(home, userLogged);
        AddValidHomePermission(otherPermissionName, member);
        var routeValues = new RouteValueDictionary
        {
            { "homeId", home.Id.ToString() }
        };
        var attribute = new HomeAuthorizationAttribute(permissionName);

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);
        _httpContextMock
            .Setup(hc => hc.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(h => h.Request.RouteValues)
            .Returns(routeValues);
        _homeServiceMock
            .Setup(h => h.GetOrDefaultMemberByHomeAndEmail(home.Id, userLogged.Email!))
            .Returns(member);

        attribute.OnAuthorization(_context);

        _context.Result.Should().BeOfType<ObjectResult>();
        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetDetails(concreteResponse.Value!).Should().Be($"Missing permission {permissionName}");
    }

    #region GetInfo

    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }

    private string GetDetails(object value)
    {
        return value.GetType().GetProperty("Details").GetValue(value).ToString();
    }
    #endregion

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "#Adf123456"
        };
    }

    private Home GetValidHome()
    {
        return new Home()
        {
            OwnerEmail = "alejofraga22v2@gmail.com",
            Coordinates = new Coordinates("123", "456"),
            Location = new Location("Golden street", "818"),
            MemberCount = 1,
        };
    }

    private Member GetValidMember(Home home, User user)
    {
        return new Member()
        {
            HomeId = home.Id,
            UserEmail = user.Email
        };
    }

    private void AddValidHomePermission(string homePermissionName, Member member)
    {
        var homePermission = new HomePermission() { Name = homePermissionName, MemberId = member.Id };
        member.HomePermissions.Add(homePermission);
    }
    #endregion
}
