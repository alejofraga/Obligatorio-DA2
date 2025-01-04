using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;
[ApiController]
[Route("me/roles")]
public class UserRoleController(IUserService userService) : SmartHomeControllerBase
{
    [HttpGet]
    [Authentication]
    public ObjectResult GetUserRoles()
    {
        var user = GetUserLogged();
        var roles = userService.GetRoles(user.Email!);

        return new ObjectResult(new
        {
            Data = new GetRolesReponse(roles)
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost]
    [Authentication]
    public ObjectResult AddRole(AddRoleRequest request)
    {
        var user = GetUserLogged();

        userService.AddRole(user.Email!, request.Role);
        return new ObjectResult(new
        {
            Message = "Role was successfully added",
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
