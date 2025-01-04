using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class UserController_Test
{
    private static Mock<IUserService> _userServiceMock = null!;
    private UserController _userController = null!;
    private static Mock<HttpContext> _httpContext = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _userController = new UserController(_userServiceMock.Object);
        _httpContext = new Mock<HttpContext>();
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _userServiceMock.VerifyAll();
        _httpContext.VerifyAll();
    }

    [TestMethod]
    public void CreateUserWithRole_WhenUserDoesNotExist_ShouldReturnOk()
    {
        var registerCompanyOwnerRequest = new CreateUserRequest
        {
            Email = "sebavega@gmail.com",
            Password = "pa$s32Word",
            Name = "seba",
            Lastname = "vega",
            Role = "companyOwner"
        };
        var newUser = new User()
        {
            Name = "Alejo",
            Lastname = "Fraga",
            Email = "alejofraga22v2@gmail.com",
            Password = "pa$s32Word",
            ProfilePicturePath = "path",
        };

        _userServiceMock
            .Setup(us => us.CreateUserWithRole(It.IsAny<CreateUserWithRoleArgs>()))
            .Returns(newUser);

        var result = _userController.CreateUserWithRole(registerCompanyOwnerRequest);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseStatusCode = objectResult.StatusCode;
        responseStatusCode.Should().NotBeNull();
        responseStatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [TestMethod]
    public void GetUsers_WhenFilterIsOk_ShouldReturnUsersFiltered()
    {
        var filter = new GetUsersFilterRequest
        {
            Role = "companyOwner",
            Fullname = "Alejo Fraga"
        };
        var role = new Role()
        {
            Name = "companyOwner",
            SystemPermissions =
            [
                new SystemPermission() { Name = "permission1" },
                new SystemPermission() { Name = "permission2" }
            ]
        };
        var user = new User()
        {
            Name = "Alejo",
            Lastname = "Fraga",
            Email = "alejofraga22v2@gmail.com",
            Password = "pa$s32Word",
            ProfilePicturePath = "path",
            Roles = [role],
            AccountCreation = DateTime.Now
        };

        _userServiceMock
            .Setup(us => us.GetUsersWithFilters(It.IsAny<GetUsersArgs>()))
            .Returns([user]);

        var act = _userController.GetUsers(filter);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<GetUsersResponse>;
        responseList.Should().NotBeNull();
        var userResponse = responseList!.First();
        userResponse.Should().NotBeNull();
        userResponse.Name.Should().Be(user.Name);
        userResponse.Lastname.Should().Be(user.Lastname);
        userResponse.Fullname.Should().Be($"{user.Name} {user.Lastname}");
        userResponse.AccountCreationDate.Should().Be(user.AccountCreation.ToString("d"));
        userResponse.Email.Should().Be(user.Email);
    }

    [TestMethod]
    public void RemoveAdmin_WhenUserIsAnAdmin_ShouldReturnOk()
    {
        var deleteUserRequest = new DeleteAdminRequest { Email = "seba@vega.com" };
        var user = new User { Name = "seba", Lastname = "vega", Email = "seba@vega.com", Password = "pa$s32Word" };
        var loggedUser = new User { Name = "matias", Lastname = "corvetto", Email = "mati@cor.com", Password = "pa$s32Word" };
        var admin = new User
        {
            Name = "admin",
            Lastname = "admin",
            Email = "as@as.com",
            Password = "pa$s32Word",
        };
        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext.Object
        };

        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(loggedUser);
        _userServiceMock
            .Setup(us => us.RemoveAdminIfNotHomeOwnerOrThrow(user.Email, loggedUser));

        var result = _userController.DeleteAdmin(deleteUserRequest);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("User seba@vega.com deleted successfully");
    }

    [TestMethod]
    public void GetProfilePicture_WhenUserExists_ShouldReturnProfilePicturePath()
    {
        var userEmail = "testuser@example.com";
        var profilePicturePath = "path/to/profile/picture.jpg";

        _userServiceMock
            .Setup(us => us.GetProfilePicturePath(userEmail))
            .Returns(profilePicturePath);

        var result = _userController.GetProfilePicture(userEmail);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseStatusCode = objectResult.StatusCode;
        responseStatusCode.Should().NotBeNull();
        responseStatusCode.Should().Be((int)HttpStatusCode.OK);
        var responseValue = objectResult.Value.GetType().GetProperty("Data")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be(profilePicturePath);
    }

    [TestMethod]
    public void UpdateProfilePicture_WhenCalled_ShouldReturnOk()
    {
        var userEmail = "testuser@example.com";
        var updateProfilePictureRequest = new UpdateProfilePictureRequest
        {
            ProfilePicturePath = "new/path/to/profile/picture.jpg"
        };

        _userServiceMock
            .Setup(us => us.SetProfilePicturePath(userEmail, updateProfilePictureRequest.ProfilePicturePath));

        var result = _userController.UpdateProfilePicture(userEmail, updateProfilePictureRequest);

        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        var responseStatusCode = objectResult.StatusCode;
        responseStatusCode.Should().NotBeNull();
        responseStatusCode.Should().Be((int)HttpStatusCode.OK);
        var responseValue = objectResult.Value.GetType().GetProperty("Message")?.GetValue(objectResult.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Profile picture updated successfully");
    }
}
