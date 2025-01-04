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
[Route("sensors")]
[Authentication]
public class SensorController : SmartHomeControllerBase
{
    private readonly IDeviceService _deviceService;

    public SensorController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    [Authorization(nameof(ValidSystemPermissions.CreateDevice))]
    public ObjectResult CreateSensor(CreateBasicDeviceRequest? createSensorRequest)
    {
        var args = new CreateBasicDeviceArgs()
        {
            Name = createSensorRequest.Name!,
            Description = createSensorRequest.Description!,
            ModelNumber = createSensorRequest.ModelNumber!,
            Photos = createSensorRequest.Photos,
            DeviceTypeName = nameof(ValidDeviceTypes.Sensor)
        };
        var newSensor = _deviceService.AddDevice(args, GetUserLogged());

        return new ObjectResult(new
        {
            Data = new CreateDeviceResponse(newSensor)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
