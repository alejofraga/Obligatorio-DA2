using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class CompanyController_Test
{
    private static Mock<ICompanyService> _companyService = null!;
    private static Mock<ISessionService> _sessionService = null!;
    private CompanyController _companyController = null!;
    private static Mock<HttpContext> _httpContext = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _sessionService = new Mock<ISessionService>(MockBehavior.Strict);
        _companyController = new CompanyController(_companyService.Object);
        _httpContext = new Mock<HttpContext>();
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _companyService.VerifyAll();
        _sessionService.VerifyAll();
        _httpContext.VerifyAll();
    }

    [TestMethod]
    public void CreateCompany_WhenCompanyIsCreated_ShouldReturnOk()
    {
        var userLogged = GetValidUser();
        var expectedRUT = "111111111112";
        var createCompanyRequest = new CreateCompanyRequest
        {
            Name = "companyName",
            Rut = expectedRUT,
            LogoUrl = "logo",
            Validator = "ValidatorLength"
        };
        var newCompany = new Company(createCompanyRequest.Validator)
        {
            Name = createCompanyRequest.Name,
            RUT = createCompanyRequest.Rut,
            LogoUrl = createCompanyRequest.LogoUrl,
            OwnerEmail = userLogged.Email
        };
        _companyController.ControllerContext = new ControllerContext { HttpContext = _httpContext.Object };

        _httpContext
            .Setup(hc => hc.Items[Item.UserLogged])
            .Returns(userLogged);
        _companyService
            .Setup(cs => cs.Add(It.IsAny<CreateCompanyArgs>()))
            .Returns(newCompany);

        var controllerResponse = _companyController.CreateCompany(createCompanyRequest);

        var result = controllerResponse as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseRUT = responseValue.GetType().GetProperty("RUT")?.GetValue(responseValue)?.ToString();
        responseRUT.Should().NotBeNull();
        responseRUT.Should().BeEquivalentTo(expectedRUT);
    }

    [TestMethod]
    public void GetCompanies_WhenFilterIsNotNull_ShouldReturnCreated()
    {
        var getCompaniesFilter = new GetCompaniesFilterRequest { CompanyName = "companyName", OwnerFullname = "alejo fraga" };
        var user = GetValidUser();
        const string validatorType = "ValidatorLength";
        var company = new Company(validatorType)
        {
            Name = "companyName",
            RUT = "111111111112",
            LogoUrl = "logo",
            OwnerEmail = "alejofraga22v2@gmail.com",
            Owner = user
        };
        const int expectedCompaniesCount = 1;

        _companyService
            .Setup(cs => cs.GetCompaniesWithFilters(It.IsAny<GetCompaniesArgs>()))
            .Returns([company]);

        var act = _companyController.GetCompanies(getCompaniesFilter);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<GetCompaniesResponse>;
        responseList.Should().NotBeNull();
        responseList.Count.Should().Be(expectedCompaniesCount);
        responseList.First().CompanyName.Should().Be(company.Name);
        responseList.First().OwnerEmail.Should().Be(company.OwnerEmail);
    }

    #region SampleData
    private static User GetValidUser()
    {
        return new User
        {
            Name = "Lionel",
            Lastname = "Messi",
            Email = "leomessi@gmail.com",
            Password = "8Ball@onDor"
        };
    }
    #endregion
}
