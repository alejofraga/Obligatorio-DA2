using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("devices")]
public class DeviceController(IDeviceService deviceService) : SmartHomeControllerBase
{
    [Authentication]
    [HttpGet]
    public ObjectResult GetDevices([FromQuery] GetDevicesFiltersRequest? filter)
    {
        var getDevicesArgs = new GetDevicesArgs()
        {
            Offset = filter!.Offset,
            Limit = filter.Limit,
            ModelNumber = filter.ModelNumber,
            DeviceType = filter.DeviceType,
            DeviceName = filter.Name,
            CompanyName = filter.CompanyName
        };
        var devices = deviceService.GetDevicesWithFilters(getDevicesArgs);

        var devicesData = devices.Select(device => new GetDevicesResponse(device)).ToList();

        return new ObjectResult(new
        {
            Data = devicesData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
