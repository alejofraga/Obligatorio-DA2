using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizationAttribute(string? permission = null)
    : Attribute,
    IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        var userLogged = context.HttpContext.Items[Item.UserLogged];

        var userIsNotIdentified = userLogged == null;
        if (userIsNotIdentified)
        {
            context.Result = new ObjectResult(new
            {
                Message = "You are not authenticated",
                Details = $"User is not identified"
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
            return;
        }

        var userLoggedMapped = (User)userLogged!;

        var permission = BuildPermission(context);

        var hasNotPermission = !userLoggedMapped.HasPermission(permission);

        if (hasNotPermission)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Access is forbidden",
                Details = $"Missing permission {permission}"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
        }
    }

    private string BuildPermission(AuthorizationFilterContext context)
    {
        return permission ?? $"{context.RouteData.Values["action"].ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}";
    }
}
