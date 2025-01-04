using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class UserRoleController_Test
{
    private static Mock<IUserService> _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
    private static Mock<HttpContext> _httpContext = null!;
    private UserRoleController _userRoleController = new(_userServiceMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _userRoleController = new UserRoleController(_userServiceMock.Object);
        _httpContext = new Mock<HttpContext>();
        _userRoleController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext.Object
        };
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _userServiceMock.VerifyAll();
        _httpContext.VerifyAll();
    }

    [TestMethod]
    public void GetRoles_WhenUserFoundAndIsAuthenticates_ShouldReturnRoles()
    {
        var getRolesRequest = Guid.NewGuid();
        var adminRole = new Role { Name = "admin" };
        var loggedUser = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Password = "DY&t3491027253",
            Name = "Alejo",
            Lastname = "Fraga",
            Roles = [adminRole]
        };

        _userServiceMock
            .Setup(us => us.GetRoles(loggedUser.Email))
            .Returns(loggedUser.Roles);
        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(loggedUser);

        var act = _userRoleController.GetUserRoles();
        var result = act as ObjectResult;
        result.Should().NotBeNull();

        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().BeOfType<GetRolesReponse>();
        ((GetRolesReponse)responseValue).Roles.Should().NotBeNull();
        ((GetRolesReponse)responseValue).Roles.Should().HaveCount(1).And.Contain("admin");
    }

    [TestMethod]
    public void AddRole_RoleToUserLogged_WhenUserFound_ShouldAddRole()
    {
        var token = Guid.NewGuid();
        var addRoleRequest = new AddRoleRequest() { Role = "homeOwner" };
        var loggedUser = new User()
        {
            Email = "alejofraga22v2@gmail.com",
            Password = "DY&t3491027253",
            Name = "Alejo",
            Lastname = "Fraga",
            Roles = []
        };

        _userServiceMock
            .Setup(us => us.AddRole(loggedUser.Email!, addRoleRequest.Role));
        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(loggedUser);

        var act = _userRoleController.AddRole(addRoleRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Role was successfully added");
    }
}
