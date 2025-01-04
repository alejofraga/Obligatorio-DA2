using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController(ISessionService sessionService) : SmartHomeControllerBase
{
    [HttpPost]
    public ObjectResult Login(LoginRequest? loginRequest)
    {
        var session = sessionService.GetOrCreateSession(loginRequest.Email, loginRequest.Password);

        return new ObjectResult(new
        {
            Data = new LoginResponse(session)
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
