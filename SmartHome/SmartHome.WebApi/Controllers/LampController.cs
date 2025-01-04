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
[Route("lamps")]
[Authentication]
public class LampController : SmartHomeControllerBase
{
    private readonly IDeviceService _deviceService;

    public LampController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    [Authorization(nameof(ValidSystemPermissions.CreateDevice))]
    public ObjectResult CreateLamp(CreateBasicDeviceRequest? createLampRequest)
    {
        var args = new CreateBasicDeviceArgs()
        {
            Name = createLampRequest!.Name,
            Description = createLampRequest.Description,
            ModelNumber = createLampRequest.ModelNumber,
            Photos = createLampRequest.Photos,
            DeviceTypeName = nameof(ValidDeviceTypes.Lamp)
        };
        var newLamp = _deviceService.AddDevice(args, GetUserLogged());

        return new ObjectResult(new
        {
            Data = new CreateDeviceResponse(newLamp)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
