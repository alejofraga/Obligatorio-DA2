using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.DeviceTypes;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;
[ApiController]
[Route("deviceTypes")]
public class DeviceTypesController(IDeviceTypesService service) : SmartHomeControllerBase
{
    [HttpGet]
    [Authentication]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
    public ObjectResult GetDeviceTypes()
    {
        var deviceTypes = service.GetDeviceTypes();

        var deviceTypesData = deviceTypes.Select(deviceType => new GetDeviceTypesResponse(deviceType)).ToList();

        return new ObjectResult(new
        {
            Data = deviceTypesData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
