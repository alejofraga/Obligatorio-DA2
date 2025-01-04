using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class HomeOwnerController_Test
{
    private static Mock<IUserService> _userServiceMock = null!;
    private HomeOwnerController _homeOwnerController = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _homeOwnerController = new HomeOwnerController(_userServiceMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _userServiceMock.VerifyAll();
    }

    [TestMethod]
    public void CreateHomeOwner_WhenUserDoesNotExist_ShouldReturnOk()
    {
        var createHomeOwnerRequest = new CreateHomeOwnerRequest
        {
            Email = "seba@vega.com",
            Password = "pa$s32Word",
            Name = "seba",
            Lastname = "vega",
            ProfilePicturePath = "path"
        };
        var newHomeOwner = new User()
        {
            Email = createHomeOwnerRequest.Email,
            Password = createHomeOwnerRequest.Password,
            Name = createHomeOwnerRequest.Name,
            Lastname = createHomeOwnerRequest.Lastname,
            ProfilePicturePath = createHomeOwnerRequest.ProfilePicturePath
        };

        _userServiceMock
            .Setup(us => us.CreateHomeOwner(It.IsAny<CreateHomeOwnerArgs>()))
            .Returns(newHomeOwner);

        var controllerResponse = _homeOwnerController.CreateHomeOwner(createHomeOwnerRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = (CreateHomeOwnerResponse)result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Email.Should().Be(newHomeOwner.Email);
        responseValue.Fullname.Should().Be(newHomeOwner.Name + " " + newHomeOwner.Lastname);
        responseValue.ProfilePicturePath.Should().Be(newHomeOwner.ProfilePicturePath);
        responseValue.AccountCreationDate.Should().Be(newHomeOwner.AccountCreation.ToString("d"));
    }
}
