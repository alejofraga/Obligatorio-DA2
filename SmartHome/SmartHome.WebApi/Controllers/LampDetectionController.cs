using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("lamps/{hardwareId}")]

public class LampDetectionController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPost("turnOn")]
    public ObjectResult GenerateTurnOnNotification(Guid hardwareId)
    {
        const bool ON = true;

        homeService.ExistHardwareOrThrow(hardwareId);

        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Lamp.ToString());
        homeService.AssertHardwareIsConnected(hardwareId);
        var message = "Lamp status changed: on";

        homeService.SendNotificationIfLampStateChanged(hardwareId, message, ON);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost("turnOff")]
    public ObjectResult GenerateTurnOffNotification(Guid hardwareId)
    {
        const bool OFF = false;

        homeService.ExistHardwareOrThrow(hardwareId);

        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Lamp.ToString());
        homeService.AssertHardwareIsConnected(hardwareId);
        var message = "Lamp status changed: off";

        homeService.SendNotificationIfLampStateChanged(hardwareId, message, OFF);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
