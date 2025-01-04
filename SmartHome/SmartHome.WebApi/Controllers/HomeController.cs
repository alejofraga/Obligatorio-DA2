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
[Route("homes/{homeId}")]
[Authentication]
public class HomeController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPatch("members")]
    [Authorization(nameof(ValidSystemPermissions.AddMember))]
    public ObjectResult AddMember(Guid homeId, AddMemberRequest? addMemberRequest)
    {
        homeService.AssertUserLoggedIsHomeOwner(homeId, GetUserLogged());

        homeService.AddMember(homeId, addMemberRequest!.UserEmail!);

        return new ObjectResult(new
        {
            Message = "Member added to home successfully",
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("members")]
    [Authorization(nameof(ValidSystemPermissions.GetMembers))]
    public ObjectResult GetMembers(Guid homeId)
    {
        var memberUsers = homeService.GetMembers(homeId);

        var membersData = memberUsers.Select(member => new GetMembersResponse(member)).ToList();

        return new ObjectResult(new
        {
            Data = membersData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("hardwares")]
    [HomeAuthorization(nameof(ValidHomePermissions.ListDevices))]
    public ObjectResult GetHardwares(Guid homeId, [FromQuery] GetHardwaresFilterRequest? getHardwaresFilterRequest)
    {
        var hardwaresData = homeService.GetHardwaresAsHardwareData(homeId, getHardwaresFilterRequest!.RoomName);

        return new ObjectResult(new
        {
            Data = hardwaresData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("hardwares")]
    [HomeAuthorization(nameof(ValidHomePermissions.AddDevice))]
    public ObjectResult AddHardware(Guid homeId, AddHardwareRequest? addHardwareRequest)
    {
        homeService.AddHardware(homeId, addHardwareRequest.ModelNumber);

        return new ObjectResult(new
        {
            Message = "Device added to home successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("permissions")]
    [HomeAuthorization(nameof(ValidHomePermissions.GrantHomePermissions))]
    public ObjectResult GrantHomePermission(Guid homeId, GrantHomePermissionRequest? grantHomePermissionRequest)
    {
        homeService.AddPermissionsToMember(homeId, grantHomePermissionRequest.MemberEmail,
            grantHomePermissionRequest.HomePermissions!);

        return new ObjectResult(new
        {
            Message = "Home permission granted successfully",
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost("rooms")]
    [HomeAuthorization(nameof(ValidHomePermissions.CreateRoom))]
    public ObjectResult AddRoom(Guid homeId, AddRoomRequest? addRoomRequest)
    {
        var roomArgs = new CreateRoomArgs()
        {
            Name = addRoomRequest!.Name,
            HardwareIds = addRoomRequest.HardwareIds,
            HomeId = homeId
        };
        var room = homeService.AddRoom(roomArgs);

        return new ObjectResult(new
        {
            Data = new CreateRoomResponse(room)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    [HttpGet("rooms")]
    public ObjectResult GetRooms(Guid homeId)
    {
        var rooms = homeService.GetRooms(homeId);
        return new ObjectResult(new
        {
            Data = new GetRoomsResponse(rooms)
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("name")]
    [HomeAuthorization(nameof(ValidHomePermissions.ChangeHomeName))]
    public ObjectResult ChangeHomeName(Guid homeId, ChangeHomeNameRequest? changeHomeNameRequest)
    {
        homeService.SetHomeName(homeId, changeHomeNameRequest!.Name);

        return new ObjectResult(new
        {
            Message = "Home name changed successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("members/permissions")]
    public ObjectResult GetMemberPermissions(Guid homeId)
    {
        var user = GetUserLogged();

        var permissions = homeService.GetMemberPermissions(homeId, user.Email!);

        return new ObjectResult(new
        {
            Data = permissions.Select(p => p.Name).ToList()
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("owner")]
    public ObjectResult GetHomeOwner(Guid homeId)
    {
        var user = GetUserLogged();

        var isOwner = homeService.UserIsTheHomeOwner(homeId, user.Email!);

        return new ObjectResult(new
        {
            Data = isOwner
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
