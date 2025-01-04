using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Companies;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("validators")]
public class ValidatorsController(ICompanyService companyService) : ControllerBase
{
    [HttpGet]
    [Authentication]
    [Authorization("createCompany")]

    public ObjectResult GetValidators()
    {
        var validatorsList = companyService.GetValidators();
        return new ObjectResult(new
        {
            Data = validatorsList
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
