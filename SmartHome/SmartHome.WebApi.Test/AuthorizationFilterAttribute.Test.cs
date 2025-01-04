using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthorizationFilterAttribute_Test
{
    private Mock<HttpContext> _httpContextMock = null!;
    private AuthorizationFilterContext _context = null!;

    [TestInitialize]
    public void Initialize()
    {
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);

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
    }

    [TestMethod]
    public void OnAuthorization_WhenUserNotAuthenticated_ShouldReturnUnauthorized()
    {
        var attribute = new AuthorizationAttribute();

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(null);

        attribute.OnAuthorization(_context);

        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetMessage(concreteResponse.Value!).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserHasNoPermission_ShouldReturnForbidden()
    {
        var userLogged = GetValidUserWithRole();
        var permissionName = "createAdmin";
        var attribute = new AuthorizationAttribute(permissionName);

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);

        attribute.OnAuthorization(_context);

        _context.Result.Should().BeOfType<ObjectResult>();
        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetMessage(concreteResponse.Value!).Should().Be($"Access is forbidden");
    }

    [TestMethod]
    public void OnAuthorization_WhenUserHasPermission_ShouldContinue()
    {
        var permissionName = "createAdmin";
        var createAdminPermission = GetValidSystemPermission(permissionName);
        var adminRole = GetValidRole("admin", [createAdminPermission]);
        var userLogged = GetValidUserWithRole([adminRole]);
        var attribute = new AuthorizationAttribute(permissionName);

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);

        attribute.OnAuthorization(_context);

        _context.Result.Should().BeNull();
    }

    [TestMethod]
    public void OnAuthorization_WhenUserHasWrongPermission_ShouldReturnForbidden()
    {
        var permissionName = "createCompany";
        var otherPermissionName = "createAdmin";
        var createCompanyPermission = GetValidSystemPermission(otherPermissionName);
        var companyOwnerRole = GetValidRole("companyOwner", [createCompanyPermission]);
        var userLogged = GetValidUserWithRole([companyOwnerRole]);
        var attribute = new AuthorizationAttribute(permissionName);

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(userLogged);

        attribute.OnAuthorization(_context);

        _context.Result.Should().BeOfType<ObjectResult>();
        var concreteResponse = _context.Result as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetMessage(concreteResponse.Value!).Should().Be($"Access is forbidden");
    }

    #region GetInfo
    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }
    #endregion

    #region SampleData
    private static User GetValidUserWithRole(List<Role>? roles = null)
    {
        return new User
        {
            Name = "Lionel",
            Lastname = "Messi",
            Email = "leomessi@gmail.com",
            Password = "8Ball@onDor",
            Roles = roles ?? []
        };
    }

    private Role GetValidRole(string roleName, List<SystemPermission> permisisons)
    {
        return new Role()
        {
            Name = roleName,
            SystemPermissions = permisisons
        };
    }

    private SystemPermission GetValidSystemPermission(string permissionName)
    {
        return new SystemPermission()
        {
            Name = permissionName
        };
    }
    #endregion
}
