using FluentAssertions;
using SmartHome.BusinessLogic.Companies;

namespace SmartHome.BusinessLogic.Test.Companies;

[TestClass]
public class Company_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateCompany()
    {
        const string expectedName = "CamerasSA";
        const string expectedLogoUrl = "www.cameraSA.com";
        const string expectedRut = "800450300128";
        var expectedEmail = "maticor93@gmail.com";
        const string validatorType = "ValidatorLength";

        var newCompany = new Company(validatorType)
        {
            LogoUrl = expectedLogoUrl,
            Name = expectedName,
            RUT = expectedRut,
            OwnerEmail = expectedEmail
        };

        newCompany.Should().NotBeNull();
        newCompany.Name.Should().Be(expectedName);
        newCompany.LogoUrl.Should().Be(expectedLogoUrl);
        newCompany.RUT.Should().Be(expectedRut);
        newCompany.OwnerEmail.Should().Be(expectedEmail);
    }

    [TestMethod]
    [DataRow("12")]
    [DataRow("abcdefghijlm")]
    [DataRow("1234567890ab")]
    [DataRow("12345678901234567890")]
    public void Create_WhenRutFormatIsIncorrect_ShouldThrowRutFormatException(string rut)
    {
        const string validatorType = "ValidatorLength";

        var act = () => new Company(validatorType)
        {
            Name = "Amazon",
            RUT = rut,
            LogoUrl = "www.photo.com",
            OwnerEmail = "maticor93@gmail.com"
        };

        act.Should().Throw<FormatException>().WithMessage("RUT number must be a sequence of 12 digits");
    }

    [TestMethod]
    [DataRow("", "800450300128", "www.photo.com", "Name", "Name")]
    [DataRow("    ", "800450300128", "www.photo.com", "Name", "Name")]
    [DataRow(null, "800450300128", "www.photo.com", "Name", "Name")]
    [DataRow("Amazon", "", "www.photo.com", "RUT", "RUT")]
    [DataRow("Amazon", "    ", "www.photo.com", "RUT", "RUT")]
    [DataRow("Amazon", "800450300128", "", "LogoUrl", "Logo url")]
    [DataRow("Amazon", "800450300128", "    ", "LogoUrl", "Logo url")]
    [DataRow("Amazon", "800450300128", null, "LogoUrl", "Logo url")]
    public void Create_WhenRequiredInfoIsEmpty_ShouldThrowException(string name, string rut, string logoUrl, string parameterName, string parameterAlias)
    {
        const string validatorType = "ValidatorLength";

        var act = () => new Company(validatorType)
        {
            Name = name,
            RUT = rut,
            LogoUrl = logoUrl,
            OwnerEmail = "maticor93@gmail.com"
        };

        act.Should().Throw<ArgumentNullException>().WithMessage($"{parameterAlias} cannot be empty (Parameter '{parameterName}')");
    }
}
