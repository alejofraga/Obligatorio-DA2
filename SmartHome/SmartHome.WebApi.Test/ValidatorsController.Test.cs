using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Companies;
using SmartHome.WebApi.Controllers;

namespace SmartHome.WebApi.Test;

[TestClass]
public class ValidatorsController_Test
{
    private static Mock<ICompanyService> _companyService = null!;
    private ValidatorsController _validatorsController = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _companyService = new Mock<ICompanyService>(MockBehavior.Strict);
        _validatorsController = new ValidatorsController(_companyService.Object);
    }

    [TestMethod]
    public void GetValidators_WhenUserIsAuthorized_ShouldGetValidators()
    {
        List<string> validators = ["validator1", "validato2"];

        _companyService.Setup(us => us.GetValidators()).Returns(validators);
        var act = _validatorsController.GetValidators();
        var result = act as ObjectResult;
        result.Should().NotBeNull();

        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().BeEquivalentTo(validators);
    }
}
