using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Companies;

[TestClass]
public class CompanyService_Test
{
    private static Mock<ICompanyRepository> _companyRepositoryMock = new Mock<ICompanyRepository>(MockBehavior.Strict);
    private CompanyService _companyService = new CompanyService(_companyRepositoryMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>(MockBehavior.Strict);
        _companyService = new CompanyService(_companyRepositoryMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _companyRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Add_WhenInfoIsCorrect_ShouldAddCompany()
    {
        var createCompanyArgs = GetValidCompanyArgs();
        var ownerEmail = createCompanyArgs.OwnerEmail;

        _companyRepositoryMock
            .Setup(repo => repo.Exist(c => c.OwnerEmail.ToUpper() == ownerEmail.ToUpper()))
            .Returns(() => false);
        _companyRepositoryMock
            .Setup(repo => repo.Exist(c => c.RUT == createCompanyArgs.RUT))
            .Returns(() => false);
        _companyRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Company>()));

        var newCompany = _companyService.Add(createCompanyArgs);

        newCompany.Should().NotBeNull();
        newCompany.Name.Should().Be(createCompanyArgs.Name);
        newCompany.RUT.Should().Be(createCompanyArgs.RUT);
        newCompany.LogoUrl.Should().Be(createCompanyArgs.LogoUrl);
        newCompany.OwnerEmail.Should().Be(createCompanyArgs.OwnerEmail);
    }

    [TestMethod]
    public void Add_WhenUserAlreadyOwnsACompany_ShouldThrowInvalidOperationException()
    {
        var newCompanyArgs = GetValidCompanyArgs();
        var email = newCompanyArgs.OwnerEmail;

        _companyRepositoryMock
            .Setup(repo => repo.Exist(c => c.OwnerEmail.ToUpper() == email.ToUpper()))
            .Returns(true);

        var act = () => _companyService.Add(newCompanyArgs);

        act.Should().Throw<InvalidOperationException>().WithMessage("User already owns a company");
    }

    [TestMethod]
    public void GetCompaniesWithFilters_WhenIsCalled_ShouldReturnAllCompanies()
    {
        var firstCompany = GetValidCompany();
        const string validatorType = "ValidatorLength";
        var secondCompany = new Company(validatorType)
        {
            OwnerEmail = GetValidUser().Email,
            Name = "Ebay",
            LogoUrl = "www.ebay.com",
            RUT = "746450570497"
        };
        var getCompaniesArgs = new GetCompaniesArgs()
        {
            Offset = 0,
            Limit = 25
        };

        _companyRepositoryMock
            .Setup(repo => repo.GetCompaniesWithFilters(It.IsAny<GetCompaniesArgs>()))
            .Returns([firstCompany, secondCompany]);

        var obtainedCompanies = _companyService.GetCompaniesWithFilters(getCompaniesArgs);

        obtainedCompanies.Should().Equal([firstCompany, secondCompany]);
    }

    [TestMethod]
    public void Add_WhenRutIsDuplicated_ShouldThrowArgumentException()
    {
        var duplicatedCompany = GetValidCompany();
        var duplicatedCompanyArgs = GetValidCompanyArgs();
        var ownerEmail = duplicatedCompany.OwnerEmail;

        _companyRepositoryMock
            .Setup(repo => repo.Exist(c => c.OwnerEmail.ToUpper() == ownerEmail.ToUpper()))
            .Returns(false);
        _companyRepositoryMock
            .Setup(repo => repo.Exist(c => c.RUT == duplicatedCompanyArgs.RUT))
            .Returns(true);

        var act = () => _companyService.Add(duplicatedCompanyArgs);

        act.Should().Throw<InvalidOperationException>().WithMessage("RUT number already in use");
    }

    #region SampleData
    private static User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }

    private static Company GetValidCompany()
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            OwnerEmail = GetValidUser().Email,
            Name = "Amazon",
            LogoUrl = "www.amazon.com",
            RUT = "800450570128"
        };
    }

    private static CreateCompanyArgs GetValidCompanyArgs()
    {
        return new CreateCompanyArgs()
        {
            Name = "Amazon",
            LogoUrl = "www.amazon.com",
            RUT = "800450570128",
            OwnerEmail = GetValidUser().Email,
            Validator = "ValidatorLength"
        };
    }
    #endregion

    [TestMethod]
    public void GetValidators_WhenOk_ShouldGetValidators()
    {
        const string folderName = "Validators";
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        Directory.CreateDirectory(path);
        List<string> expectedValidators = [];

        var actualValidators = _companyService.GetValidators();

        actualValidators.Should().BeEquivalentTo(expectedValidators);
        Directory.Delete(path, true);
    }
}
