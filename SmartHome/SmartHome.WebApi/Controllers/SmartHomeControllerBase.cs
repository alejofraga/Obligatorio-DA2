using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

public class SmartHomeControllerBase : ControllerBase
{
    protected User GetUserLogged()
    {
        var userLogged = HttpContext.Items[Item.UserLogged];

        var userLoggedMapped = (User)userLogged;

        if (userLoggedMapped == null)
        {
            throw new UnauthorizedAccessException("Session expired");
        }

        return userLoggedMapped;
    }
}
