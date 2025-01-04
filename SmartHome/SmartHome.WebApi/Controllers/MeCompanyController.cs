using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Companies;
using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Controllers;
[ApiController]
[Route("/me/imOwner")]
[Authentication]
public class MeCompanyController(ICompanyService companyService) : SmartHomeControllerBase
{
    public ObjectResult GetImOwner()
    {
        var user = GetUserLogged();
        var response = companyService.UserAlreadyOwnsAcompany(user.Email!);
        return new ObjectResult(new
        {
            Data = new
            {
                IsOwner = response
            }
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
