using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : SmartHomeControllerBase
{
    [HttpPost]
    [Authentication]
    [Authorization(nameof(ValidSystemPermissions.CreateUserWithRole))]
    public ObjectResult CreateUserWithRole(CreateUserRequest? createUserRequest)
    {
        var createUserWithRoleArgs = new CreateUserWithRoleArgs()
        {
            Email = createUserRequest!.Email,
            Name = createUserRequest.Name,
            Lastname = createUserRequest.Lastname,
            Password = createUserRequest.Password,
            Role = createUserRequest.Role
        };
        var newUser = userService.CreateUserWithRole(createUserWithRoleArgs);

        return new ObjectResult(new
        {
            Data = new CreateUserWithRoleResponse(newUser)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    [HttpGet]
    [Authentication]
    [Authorization(nameof(ValidSystemPermissions.GetUsers))]
    public ObjectResult GetUsers([FromQuery] GetUsersFilterRequest? filter)
    {
        var getAllUserArgs = new GetUsersArgs()
        {
            Role = filter!.Role,
            FullName = filter.Fullname,
            Offset = filter.Offset,
            Limit = filter.Limit
        };
        var users = userService.GetUsersWithFilters(getAllUserArgs);

        var usersData = users.Select(user => new GetUsersResponse(user)).ToList();

        return new ObjectResult(new
        {
            Data = usersData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpDelete]
    [Authentication]
    [Authorization(nameof(ValidSystemPermissions.DeleteAdmin))]
    public ObjectResult DeleteAdmin(DeleteAdminRequest? deleteAdminRequest)
    {
        userService.RemoveAdminIfNotHomeOwnerOrThrow(deleteAdminRequest!.Email, GetUserLogged());

        return new ObjectResult(new
        {
            Message = $"User {deleteAdminRequest.Email} deleted successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("{userEmail}/picture")]
    [Authentication]
    public ObjectResult GetProfilePicture(string userEmail)
    {
        var profilePicturePath = userService.GetProfilePicturePath(userEmail);

        return new ObjectResult(new
        {
            Data = profilePicturePath
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("{userEmail}/picture")]
    [Authentication]
    public ObjectResult UpdateProfilePicture(string userEmail, [FromBody] UpdateProfilePictureRequest? updateProfilePictureRequest)
    {
        userService.SetProfilePicturePath(userEmail, updateProfilePictureRequest.ProfilePicturePath);

        return new ObjectResult(new
        {
            Message = "Profile picture updated successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
