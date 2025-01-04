using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthenticationAttribute
    : Attribute,
        IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                SetUnauthorizedResult(context);
            }

            var userOfAuthorization = GetUserOfAuthorization(authorizationHeader!, context);

            context.HttpContext.Items[Item.UserLogged] = userOfAuthorization;
        }
        catch (Exception)
        {
            SetUnauthorizedResult(context);
        }
    }

    private User GetUserOfAuthorization(string authorization, AuthorizationFilterContext context)
    {
        var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();

        var user = sessionService.GetUserBySessionId(authorization);

        return user;
    }

    private void SetUnauthorizedResult(AuthorizationFilterContext context)
    {
        context.Result = new ObjectResult(new
        {
            Message = "You are not authenticated",
            Details = $"User is not identified"
        })
        { StatusCode = (int)HttpStatusCode.Unauthorized };
    }
}
