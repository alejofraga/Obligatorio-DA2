using FluentAssertions;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.DataLayer.Test;
[TestClass]
public class SessionRepository_Test
{
    private SmartHomeDbContext _context = DbContextBuilder.BuildSmartHomeDbContext();
    private SessionRepository _sessionRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _context = DbContextBuilder.BuildSmartHomeDbContext();
        _sessionRepository = new SessionRepository(_context);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetOrDefault_ShouldReturnSession_WhenSessionExists()
    {
        var user = new User { Email = "alejofraga22v2@gmail.com", Name = "Test User", Password = "@GGags65gGGs7221", Lastname = "fraga" };
        var session = new Session { UserEmail = user.Email };
        _context.Users.Add(user);
        _context.Sessions.Add(session);
        _context.SaveChanges();
        var result = _sessionRepository.GetOrDefault(s => s.UserEmail == user.Email);
        result.Should().BeEquivalentTo(session);
    }
}
