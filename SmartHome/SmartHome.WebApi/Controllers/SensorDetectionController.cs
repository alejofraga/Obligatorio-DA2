using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("sensors/{hardwareId}")]

public class SensorDetectionController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPost("windowOpened")]
    public ObjectResult GenerateWindowOpenedNotification(Guid hardwareId)
    {
        const bool opened = true;

        homeService.ExistHardwareOrThrow(hardwareId);
        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Sensor.ToString());
        homeService.AssertHardwareIsConnected(hardwareId);
        var message = "Window movement: opened";

        homeService.SendNotificationIfSensorStateChanged(hardwareId, message, opened);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost("windowClosed")]
    public ObjectResult GenerateWindowClosedNotiication(Guid hardwareId)
    {
        const bool CLOSED = false;

        homeService.ExistHardwareOrThrow(hardwareId);
        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Sensor.ToString());
        homeService.AssertHardwareIsConnected(hardwareId);
        var message = "Window movement: closed";

        homeService.SendNotificationIfSensorStateChanged(hardwareId, message, CLOSED);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
