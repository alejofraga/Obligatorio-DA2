using FluentAssertions;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class CompanyRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private CompanyRepository _companyRepostitory = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _companyRepostitory = new CompanyRepository(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenInfoIsCorrect_ShouldReturnFilteredDevicesWithOffsetAndLimit()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        _companyRepostitory.Add(firstCompany);
        _companyRepostitory.Add(secondCompany);
        var getCompaniesArgs = new GetCompaniesArgs()
        {
            Offset = 1,
            Limit = 1
        };

        var result = _companyRepostitory.GetCompaniesWithFilters(getCompaniesArgs);

        result.Should().BeEquivalentTo([secondCompany]);
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenAllFiltersAreCorrect_ShouldReturnFilteredDevices()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        _companyRepostitory.Add(firstCompany);
        _companyRepostitory.Add(secondCompany);
        var getCompaniesArgs = new GetCompaniesArgs()
        {
            Offset = 0,
            Limit = 5,
            CompanyName = firstCompany.Name,
            OwnerFullname = $"{firstCompany.Owner.Name} {firstCompany.Owner.Lastname}"
        };

        var result = _companyRepostitory.GetCompaniesWithFilters(getCompaniesArgs);

        result.Should().BeEquivalentTo([firstCompany]);
    }

    [TestMethod]
    public void GetDevicesWithFilters_WhenFiltersAreIncorrect_ShouldReturnEmptyList()
    {
        var firstCompany = GetFirstValidCompany();
        var secondCompany = GetSecondValidCompany();
        _companyRepostitory.Add(firstCompany);
        _companyRepostitory.Add(secondCompany);
        var getCompaniesArgs = new GetCompaniesArgs()
        {
            Offset = 1,
            Limit = 1,
            CompanyName = "Wrong company name",
            OwnerFullname = "Wrong Fullname"
        };

        var result = _companyRepostitory.GetCompaniesWithFilters(getCompaniesArgs);

        result.Should().BeEmpty();
    }

    #region SampleData
    private User GetFirstValidUser()
    {
        var user = new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
        _context.Add(user);

        return user;
    }

    private User GetSecondValidUser()
    {
        var user = new User()
        {
            Email = "messi@gmail.com",
            Name = "Lionel",
            Lastname = "Fressi",
            Password = "#Adf123456"
        };
        _context.Add(user);

        return user;
    }

    private Company GetFirstValidCompany()
    {
        const string validatorType = "ValidatorLength";
        var firstUser = GetFirstValidUser();

        var company = new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = firstUser.Email,
            Owner = firstUser
        };

        return company;
    }

    private Company GetSecondValidCompany()
    {
        const string validatorType = "ValidatorLength";
        var secondUser = GetSecondValidUser();

        var company = new Company(validatorType)
        {
            Name = "SensorSA",
            RUT = "800450300728",
            LogoUrl = "www.sensorSA.com",
            OwnerEmail = secondUser.Email,
            Owner = secondUser
        };

        return company;
    }
    #endregion
}
