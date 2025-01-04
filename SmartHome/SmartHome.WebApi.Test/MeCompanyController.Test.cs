using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class MeCompanyController_Test
{
    private MeCompanyController _controller = null!;
    private Mock<ICompanyService> _companyOwnerServiceMock = null!;
    private static Mock<HttpContext> _httpContextMock = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _companyOwnerServiceMock = new Mock<ICompanyService>(MockBehavior.Strict);
        _controller = new MeCompanyController(_companyOwnerServiceMock.Object);
        _httpContextMock = new Mock<HttpContext>();
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _companyOwnerServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void GetImOwner_WhenUserIsOwner_ShouldReturnOk()
    {
        var userLogged = new User { Email = "co@smarthome.com", Password = "pa$s32Word", Name = "co", Lastname = "oc" };
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(userLogged);
        _companyOwnerServiceMock
            .Setup(c => c.UserAlreadyOwnsAcompany(userLogged.Email!))
            .Returns(true);
        _controller.ControllerContext.HttpContext = _httpContextMock.Object;
        var response = _controller.GetImOwner();
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Value.Should().BeEquivalentTo(new
        {
            Data = new
            {
                IsOwner = true
            }
        });
    }
}
