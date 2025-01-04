using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("cameras")]
[Authentication]
public class CameraController : SmartHomeControllerBase
{
    private readonly IDeviceService _deviceService;

    public CameraController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    [Authorization(nameof(ValidSystemPermissions.CreateDevice))]
    public ObjectResult CreateCamera(CreateCameraRequest? createCameraRequest)
    {
        var createCameraArgs = new CreateCameraArgs()
        {
            Name = createCameraRequest!.Name,
            Description = createCameraRequest.Description,
            ModelNumber = createCameraRequest.ModelNumber,
            Photos = createCameraRequest.Photos,
            HasMovementDetection = createCameraRequest.HasMovementDetection,
            HasPersonDetection = createCameraRequest.HasPersonDetection,
            IsOutdoor = createCameraRequest.IsOutdoor,
            IsIndoor = createCameraRequest.IsIndoor
        };
        var newCamara = _deviceService.AddCamera(createCameraArgs, GetUserLogged());

        return new ObjectResult(new
        {
            Data = new CreateCameraResponse(newCamara)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
