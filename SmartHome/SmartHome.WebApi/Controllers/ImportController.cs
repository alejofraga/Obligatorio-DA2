using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Devices;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

public class ImportController(IDeviceService deviceService) : SmartHomeControllerBase
{
    [HttpGet]
    [Route("importers")]
    [Authentication]
    public ObjectResult GetImporters()
    {
        var importerNames = deviceService.GetImporterNames()
            .Select(x => new GetImportersResponseObject { Name = x });

        return new ObjectResult(new
        {
            Data = importerNames
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet]
    [Route("importers/{importerName}/params")]
    [Authentication]
    public ObjectResult GetImporterParams(string importerName)
    {
        var importerParams = deviceService.GetImporterParams(importerName);

        return new ObjectResult(new
        {
            Data = importerParams
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPost]
    [Route("importers/{importerName}")]
    [Authentication]
    public ObjectResult ImportDevices([FromBody] ImportRequest request, string importerName)
    {
        deviceService.ImportDevices(importerName, request.Params, GetUserLogged());

        return new ObjectResult(new
        {
            Message = "Devices imported successfully!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
