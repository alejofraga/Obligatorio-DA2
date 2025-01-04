using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.WebApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class HomeAuthorizationAttribute(string? permission = null)
            : Attribute,
            IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result != null)
        {
            return;
        }

        var stringHomeId = (string?)context.HttpContext.Request.RouteValues["homeId"];
        var stringHardwareId = (string?)context.HttpContext.Request.RouteValues["hardwareId"];

        if (stringHomeId == null && stringHardwareId == null)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Argument cannot be null or empty",
                Details = "Resource Id cannot be empty"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            return;
        }

        var homeService = context.HttpContext.RequestServices.GetService<IHomeService>();

        Guid homeId;
        if (stringHomeId != null)
        {
            if (!Guid.TryParse(stringHomeId, out Guid parsedHomeId))
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Argument is invalid",
                    Details = "Invalid Home Id"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            homeId = parsedHomeId;
        }
        else
        {
            if (!Guid.TryParse(stringHardwareId, out Guid parsedHardwareId))
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Argument is invalid",
                    Details = "Invalid Hardware Id"
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            var hardwares = homeService.GetHardwaresByIds([parsedHardwareId]);

            if (hardwares.Count == 0)
            {
                context.Result = new ObjectResult(new
                {
                    Message = "Element not found",
                    Details = "Hardware not found"
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };
                return;
            }

            homeId = hardwares[0].HomeId;
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

        var activeMember = homeService!.GetOrDefaultMemberByHomeAndEmail(homeId, userLoggedMapped.Email!);
        if (activeMember == null)
        {
            context.Result = new ObjectResult(new
            {
                Message = "Access is forbidden",
                Details = $"You do not belong to the home"
            })
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };
            return;
        }

        var permission = BuildPermission(context);

        var hasNotPermission = !activeMember.HasHomePermission(permission);

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
