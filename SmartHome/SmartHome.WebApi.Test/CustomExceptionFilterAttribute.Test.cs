using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Test;

[TestClass]
public class CustomExceptionFilterAttribute_Test
{
    private ExceptionContext _context = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = new ExceptionContext(
            new ActionContext(
                new Mock<HttpContext>().Object,
                new RouteData(),
                new ActionDescriptor()),
            []);
    }

    [TestMethod]
    public void OnException_WhenExceptionIsNotRegistered_ShouldResponseInternalError()
    {
        _context.Exception = new Exception("Not registered");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        GetMessage(concreteResponse.Value!).Should().Be("There was an error when processing the request");
    }

    [TestMethod]
    public void OnException_WhenArgumentNullException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentNullException("Id", "Id is invalid");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetMessage(concreteResponse.Value!).Should().Be("Argument cannot be null or empty");
        GetDetails(concreteResponse.Value!).Should().Be("Id is invalid (Parameter 'Id')");
    }

    [TestMethod]
    public void OnException_WhenArgumentException_ShouldResponseBadRequest()
    {
        _context.Exception = new ArgumentException("Id is not valid");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetMessage(concreteResponse.Value!).Should().Be("Argument is invalid");
        GetDetails(concreteResponse.Value!).Should().Be("Id is not valid");
    }

    [TestMethod]
    public void OnException_WhenFormatException_ShouldResponseBadRequest()
    {
        _context.Exception = new FormatException("RUT number must be 12 digits long");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        GetMessage(concreteResponse.Value!).Should().Be("Argument format is invalid");
        GetDetails(concreteResponse.Value!).Should().Be("RUT number must be 12 digits long");
    }

    [TestMethod]
    public void OnException_NotFoundException_ShouldResponseNotFound()
    {
        _context.Exception = new NotFoundException("Home not found");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        GetMessage(concreteResponse.Value!).Should().Be("Element not found");
        GetDetails(concreteResponse.Value!).Should().Be("Home not found");
    }

    [TestMethod]
    public void OnException_InvalidOperationException_ShouldResponseConflict()
    {
        _context.Exception = new InvalidOperationException("Email already in use");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        GetMessage(concreteResponse.Value!).Should().Be("A conflict occurred with existing resources");
        GetDetails(concreteResponse.Value!).Should().Be("Email already in use");
    }

    [TestMethod]
    public void OnException_UnauthorizedAccessException_ShouldResponseUnauthorized()
    {
        _context.Exception = new UnauthorizedAccessException();
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        GetMessage(concreteResponse.Value!).Should().Be("You are not authenticated");
    }

    [TestMethod]
    public void OnException_ForbiddenAccessException_ShouldResponseForbidden()
    {
        var permission = "ReadHome";
        _context.Exception = new ForbiddenAccessException($"Missing permission {permission}");
        var attribute = new CustomExceptionAttribute();

        attribute.OnException(_context);

        var context = _context.Result;
        context.Should().NotBeNull();
        var concreteResponse = context as ObjectResult;
        concreteResponse.Should().NotBeNull();
        concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        GetMessage(concreteResponse.Value!).Should().Be("Access is forbidden");
        GetDetails(concreteResponse.Value!).Should().Be($"Missing permission {permission}");
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
}
