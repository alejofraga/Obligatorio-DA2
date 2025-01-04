using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;

[ApiController]
[Route("companies")]
[Authentication]
public class CompanyController(ICompanyService companyService) : SmartHomeControllerBase
{
    [HttpPost]
    [Authorization(nameof(ValidSystemPermissions.CreateCompany))]
    public ObjectResult CreateCompany(CreateCompanyRequest? createCompanyRequest)
    {
        var loggedUser = GetUserLogged();

        var validator = createCompanyRequest!.Validator;

        var createCompanyArgs = new CreateCompanyArgs()
        {
            Name = createCompanyRequest!.Name,
            RUT = createCompanyRequest.Rut,
            LogoUrl = createCompanyRequest.LogoUrl,
            OwnerEmail = loggedUser.Email,
            Validator = validator
        };
        var company = companyService.Add(createCompanyArgs);

        return new ObjectResult(new
        {
            Data = new CreateCompanyResponse(company)
        })
        {
            StatusCode = (int)HttpStatusCode.Created
        };
    }

    [HttpGet]
    [Authorization(nameof(ValidSystemPermissions.GetCompanies))]
    public ObjectResult GetCompanies([FromQuery] GetCompaniesFilterRequest? filter)
    {
        var getCompaniesArgs = new GetCompaniesArgs()
        {
            Offset = filter.Offset,
            Limit = filter.Limit,
            CompanyName = filter.CompanyName,
            OwnerFullname = filter.OwnerFullname
        };
        var companies = companyService.GetCompaniesWithFilters(getCompaniesArgs);

        var companiesData = companies.Select(company => new GetCompaniesResponse(company)).ToList();

        return new ObjectResult(new
        {
            Data = companiesData
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
