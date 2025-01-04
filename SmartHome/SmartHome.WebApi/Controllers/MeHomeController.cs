using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("me/homes")]
public class MeHomeController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPost]
    [Authentication]
    [Authorization(nameof(ValidSystemPermissions.CreateHome))]
    public ObjectResult CreateHome(CreateHomeRequest? createHomeRequest)
    {
        var createHomeArgs = new CreateHomeArgs()
        {
            Owner = GetUserLogged(),
            Address = createHomeRequest.Address,
            DoorNumber = createHomeRequest.DoorNumber,
            Latitude = createHomeRequest.Latitude,
            Longitude = createHomeRequest.Longitude,
            MemberCount = createHomeRequest.MemberCount,
            Name = createHomeRequest.Name
        };
        var newHome = homeService.CreateHome(createHomeArgs);

        return new ObjectResult(new
        {
            Data = new CreateHomeResponse(newHome)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    [HttpGet]
    [Authentication]
    [Authorization(nameof(ValidSystemPermissions.GetHomes))]
    public ObjectResult GetUserHomes([FromQuery] GetHomesFilterRequest? getHomesFilterRequest)
    {
        var getUserHomesArgs = new GetUserHomesArgs
        {
            Limit = getHomesFilterRequest.Limit,
            Offset = getHomesFilterRequest.Offset,
            User = GetUserLogged()
        };
        var homes = homeService.GetHomesWithFilters(getUserHomesArgs);

        var homeData = homes.Select(home => new GetUserHomesResponse(home)).ToList();

        return new ObjectResult(new
        {
            Data = homeData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
