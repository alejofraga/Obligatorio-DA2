using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Homes;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("hardwares/{hardwareId}")]
[Authentication]

public class HardwareController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPatch("status")]
    public ObjectResult UpdateHardwareStatus(Guid hardwareId, UpdateHardwareStatusRequest request)
    {
        homeService.UpdateHardwareStatus(hardwareId, (bool)request.Connected!);

        return new ObjectResult(new
        {
            Message = "Hardware updated successfully",
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("name")]
    [HomeAuthorization(nameof(ValidHomePermissions.ChangeDeviceName))]
    public ObjectResult SetHardwareName(Guid hardwareId, SetHardwareNameRequest setHardwareNameRequest)
    {
        homeService.SetHardwareName(hardwareId, setHardwareNameRequest.Name);

        return new ObjectResult(new
        {
            Message = "Hardware name set successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch("room")]
    [HomeAuthorization(nameof(ValidHomePermissions.AddDeviceToRoom))]
    public ObjectResult SetHardwareRoom(Guid hardwareId, [FromBody] SetHardwareRoomRequest setHardwareRoomRequest)
    {
        homeService.SetHardwareRoom(setHardwareRoomRequest.RoomId, hardwareId, GetUserLogged());

        return new ObjectResult(new
        {
            Message = "Hardware added to room successfully"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
