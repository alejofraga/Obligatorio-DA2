using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("movementSensor/{hardwareId}")]

public class MovementSensorDetectionController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpPost("movementDetection")]
    public ObjectResult GenerateMovementDetectionNotification(Guid hardwareId)
    {
        homeService.ExistHardwareOrThrow(hardwareId);
        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.MovementSensor.ToString());
        homeService.AssertHardwareIsConnected(hardwareId);

        const string message = "Movement detected!";

        homeService.SendNotification(hardwareId, message);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        { StatusCode = (int)HttpStatusCode.OK };
    }
}
