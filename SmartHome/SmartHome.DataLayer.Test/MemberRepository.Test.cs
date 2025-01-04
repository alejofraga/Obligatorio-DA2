using FluentAssertions;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;

[TestClass]
public class MemberRepositoryTest
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private MemberRepository _memberRepository = null!;
    private UserRepository _userRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _memberRepository = new MemberRepository(_context);
        _userRepository = new UserRepository(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetOrDefault_WhenMemberExists_ShouldGetMember()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var company = GetValidCompany(user);
        _context.Companies.Add(company);
        _context.SaveChanges();
        _context.SaveChanges();
        var home = GetValidHome(user, 1);
        _context.Homes.Add(home);
        _context.SaveChanges();
        var member = GetValidMember(user, home);
        _memberRepository.Add(member);
        var act = _memberRepository.GetOrDefault(m => m.UserEmail == user.Email && m.HomeId == home.Id);
        act.Should().NotBeNull();
        act.Should().BeEquivalentTo(member);
    }

    [TestMethod]
    public void GetAll_WhenMembersExist_ShouldGetMembers()
    {
        var user = GetValidUser();
        _userRepository.Add(user);
        var company = GetValidCompany(user);
        _context.Companies.Add(company);
        _context.SaveChanges();
        var home = GetValidHome(user, 1);
        _context.Homes.Add(home);
        _context.SaveChanges();
        var member = GetValidMember(user, home);
        _memberRepository.Add(member);
        var act = _memberRepository.GetAll(m => m.UserEmail == user.Email && m.HomeId == home.Id);
        act.Should().NotBeNull();
        act.Should().HaveCount(1);
        act.Should().Contain(member);
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456",
            ProfilePicturePath = "pathMati"
        };
    }

    private Company GetValidCompany(User user)
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = user.Email
        };
    }

    private Member GetValidMember(User user, Home home)
    {
        return new Member()
        {
            UserEmail = user.Email,
            HomeId = home.Id
        };
    }

    private Home GetValidHome(User user, int memberCount)
    {
        return new Home()
        {
            Owner = user,
            Coordinates = new Coordinates()
            {
                Longitude = "13",
                Latitude = "12",
            },
            Location = new Location()
            {
                Address = "address",
                DoorNumber = "818"
            },
            OwnerEmail = user.Email,
            MemberCount = memberCount,
        };
    }
    #endregion
}
