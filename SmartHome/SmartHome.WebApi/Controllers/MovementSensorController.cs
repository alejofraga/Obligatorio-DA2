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
[Route("movementSensors")]
[Authentication]
public class MovementSensorController : SmartHomeControllerBase
{
    private readonly IDeviceService _deviceService;

    public MovementSensorController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    [Authorization(nameof(ValidSystemPermissions.CreateDevice))]
    public ObjectResult CreateMovementSensor(CreateBasicDeviceRequest createMovementSensorRequest)
    {
        var createMovementSensorArgs = new CreateBasicDeviceArgs()
        {
            Name = createMovementSensorRequest.Name,
            Description = createMovementSensorRequest.Description,
            ModelNumber = createMovementSensorRequest.ModelNumber,
            Photos = createMovementSensorRequest.Photos,
            DeviceTypeName = nameof(ValidDeviceTypes.MovementSensor)
        };
        var newMovementSensor = _deviceService.AddDevice(createMovementSensorArgs, GetUserLogged());

        return new ObjectResult(new
        {
            Data = new CreateDeviceResponse(newMovementSensor)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
