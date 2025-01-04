using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("homeowners")]
public class HomeOwnerController(IUserService userService) : SmartHomeControllerBase
{
    [HttpPost]
    public ObjectResult CreateHomeOwner(CreateHomeOwnerRequest? createUserRequest)
    {
        var createHomeOwnerArgs = new CreateHomeOwnerArgs()
        {
            Name = createUserRequest!.Name,
            Lastname = createUserRequest.Lastname,
            Email = createUserRequest.Email,
            Password = createUserRequest.Password,
            ProfilePicturePath = createUserRequest.ProfilePicturePath
        };
        var homeOwner = userService.CreateHomeOwner(createHomeOwnerArgs);

        return new ObjectResult(new
        {
            Data = new CreateHomeOwnerResponse(homeOwner)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
