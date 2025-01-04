using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Requests;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("cameras/{hardwareId}")]

public class CameraDetectionController(IHomeService homeService, IUserService userService) : SmartHomeControllerBase
{
    [HttpPost("movementDetection")]
    public ObjectResult GenerateMovementDetectionNotification(Guid hardwareId)
    {
        homeService.ExistHardwareOrThrow(hardwareId);
        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Camera.ToString());
        homeService.AssertCameraHasMovementDetectionFeature(hardwareId);
        homeService.AssertHardwareIsConnected(hardwareId);

        const string message = "Movement detected!";

        homeService.SendNotification(hardwareId, message);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost("personDetection")]
    public ObjectResult GeneratePersonDetectionNotification(Guid hardwareId, GenerateCameraPersonDetectionNotificationRequest? request)
    {
        homeService.ExistHardwareOrThrow(hardwareId);
        homeService.AssertIsValidDevice(hardwareId, ValidDeviceTypes.Camera.ToString());
        homeService.AssertCameraHasPersonDetectionFeature(hardwareId);
        homeService.AssertHardwareIsConnected(hardwareId);

        var identifiedUser = userService.GetByEmailOrThrow(request.IdentifiedUserEmail!, "IdentifiedUserEmail");

        var message = $"Person detected! User identified: {identifiedUser.Name} {identifiedUser.Lastname} - {identifiedUser.Email}";

        homeService.SendNotification(hardwareId, message);
        return new ObjectResult(new
        {
            Message = "Members notified successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
