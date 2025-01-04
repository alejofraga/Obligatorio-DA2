using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class AuthenticationFilterAttribute_Test
{
    private Mock<HttpContext> _httpContextMock = null!;
    private AuthorizationFilterContext _context = null!;
    private readonly AuthenticationAttribute _attribute = new AuthenticationAttribute();

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
    public void OnAuthorization_WhenEmptyHeaders_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock
            .Setup(h => h.Request.Headers)
            .Returns(new HeaderDictionary());

        _attribute.OnAuthorization(_context);

        var context = _context.Result;
        _httpContextMock.VerifyAll();
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetMessage(concreteResponse.Value!).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthenticationIsEmpty_ShouldReturnUnauthenticatedResponse()
    {
        _httpContextMock
            .Setup(h => h.Request.Headers)
            .Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", string.Empty }
            }));

        _attribute.OnAuthorization(_context);

        var context = _context.Result;
        _httpContextMock.VerifyAll();
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse!.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetMessage(concreteResponse.Value!).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnAuthorization_WhenAuthenticationIsOk_ShouldReturnUserLogged()
    {
        var expectedToken = "ba60b9e2-acc6-4673-9cb2-74b37c1b6063";
        var expectedUser = new User()
        {
            Name = "Lionel",
            Lastname = "Messi",
            Email = "leomessi@gmail.com",
            Password = "8Ball@onDor"
        };
        var sessionServiceMock = new Mock<ISessionService>();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var context = _context.Result;

        sessionServiceMock
            .Setup(s => s.GetUserBySessionId(expectedToken))
            .Returns(expectedUser);
        serviceProviderMock
            .Setup(s => s.GetService(typeof(ISessionService)))
            .Returns(sessionServiceMock.Object);
        _httpContextMock
            .Setup(h => h.Request.Headers)
            .Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", expectedToken }
            }));
        _httpContextMock
            .Setup(h => h.RequestServices)
            .Returns(serviceProviderMock.Object);
        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged])
            .Returns(expectedUser);

        _attribute.OnAuthorization(_context);

        _context.HttpContext.Items[Item.UserLogged].Should().Be(expectedUser);
        context.Should().BeNull();
        sessionServiceMock.VerifyAll();
        serviceProviderMock.VerifyAll();
    }

    #region GetInfo
    private string GetMessage(object value)
    {
        return value.GetType().GetProperty("Message").GetValue(value).ToString();
    }
    #endregion
}
