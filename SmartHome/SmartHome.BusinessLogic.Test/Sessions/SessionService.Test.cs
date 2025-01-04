using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SmartHome.BusinessLogic.Exceptions;
using SmartHome.BusinessLogic.Sessions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Sessions;

[TestClass]
public class SessionService_Test
{
    private static Mock<ISessionRepository> _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);

    private static Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

    private SessionService _sessionService = new SessionService(_sessionRepositoryMock.Object, _userRepositoryMock.Object);

    [TestInitialize]
    public void OnInitialize()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _sessionService = new SessionService(_sessionRepositoryMock.Object, _userRepositoryMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _sessionRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetOrCreateSession_WhenUserAlreadyLogged_ShouldReturnExistingSession()
    {
        var user = GetValidUser();
        var email = user.Email;
        var password = user.Password;
        var existingSession = new Session(user);

        _sessionRepositoryMock
            .Setup(repo => repo.Exist(s => s.UserEmail!.ToUpper() == email.ToUpper() && s.User.Password == password))
            .Returns(true);
        _sessionRepositoryMock
            .Setup(repo => repo.GetOrDefault(s => s.UserEmail!.ToUpper() == email.ToUpper()))
            .Returns(existingSession);

        var result = _sessionService.GetOrCreateSession(email, password);

        result.Should().Be(existingSession);
    }

    [TestMethod]
    public void GetOrCreateSession_WhenUserNotLogged_ShouldCreateNewSession()
    {
        var user = GetValidUser();
        var email = user.Email;
        var password = user.Password;
        var newSession = new Session(user);

        _sessionRepositoryMock
            .Setup(repo => repo.Exist(It.IsAny<Expression<Func<Session, bool>>>()))
            .Returns(false);
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.Is<Expression<Func<User, bool>>>(expr => expr.Compile()(user))))
            .Returns(user);
        _sessionRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<Session>()))
            .Callback<Session>(session => newSession = session);

        var result = _sessionService.GetOrCreateSession(email, password);

        result.Should().Be(newSession);
    }

    [TestMethod]
    [DataRow("maticor@gmail.com", null)]
    [DataRow(null, "Ada#123")]
    public void GetOrCreateSession_WhenEmailOrPasswordIsNull_ShouldThrowArgumentException(string email, string password)
    {
        var act = () => _sessionService.GetOrCreateSession(email, password);

        act.Should().Throw<ArgumentException>().WithMessage("Email and password are required to authenticate");
    }

    [TestMethod]
    public void GetUserBySessionId_WhenTokenIsNull_ThenThrowUnauthorizedAccessException()
    {
        var act = () => _sessionService.GetUserBySessionId(null);

        act.Should().Throw<UnauthorizedAccessException>().WithMessage("Invalid authentication token");
    }

    [TestMethod]
    public void GetUserBySessionId_WhenTokenDoesntExists_ThenThrowUnauthorizedAccessException()
    {
        const string unExistantToken = "unExistantToken";

        _sessionRepositoryMock
            .Setup(repo => repo.GetOrDefault(s => s.SessionId.ToString() == unExistantToken))
            .Returns(value: null);

        var act = () => _sessionService.GetUserBySessionId(unExistantToken);

        act.Should().Throw<NotFoundException>().WithMessage("Session not found");
    }

    [TestMethod]
    public void GetUserBySessionId_WhenTokenExists_ThenReturnUser()
    {
        var user = GetValidUser();
        var session = new Session(user);
        var sessionId = session.SessionId.ToString();

        _sessionRepositoryMock
            .Setup(repo => repo.GetOrDefault(s => s.SessionId.ToString() == sessionId))
            .Returns(session);

        var result = _sessionService.GetUserBySessionId(session.SessionId.ToString());

        result.Should().Be(user);
    }

    [TestMethod]
    public void IsUserAlreadyLogged_WhenUserIsLogged_ThenReturnTrue()
    {
        var userPassword = "as!@2DSASd21312";
        var userEmail = "Alejofraga22v2@gmail.com";
        var user = GetValidUser();
        var session = new Session(user);

        _sessionRepositoryMock
            .Setup(repo => repo.Exist(s => s.UserEmail.ToUpper() == userEmail.ToUpper() && s.User.Password == userPassword))
            .Returns(true);

        var result = _sessionService.IsUserAlreadyLogged(userEmail, userPassword);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenEmailAndPasswordAreCorrect_ShouldReturnValidUser()
    {
        var expectedUser = new User()
        {
            Email = "test@test.com",
            Name = "John",
            Lastname = "Lennon",
            Password = "1232524SASADaa!!"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(expectedUser);

        var result = _sessionService.GetValidUserOrThrow(expectedUser.Email, expectedUser.Password);

        result.Should().Be(expectedUser);
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenEmailIsNull_ShouldThrowArgumentException()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((User)null);

        var act = () => _sessionService.GetValidUserOrThrow(null, "password");

        act.Should().Throw<ArgumentException>().WithMessage("Failed to authenticate");
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenPasswordIsNull_ShouldThrowArgumentException()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(GetValidUser());

        var act = () => _sessionService.GetValidUserOrThrow("maticor93@gmail.com", null);

        act.Should().Throw<ArgumentException>().WithMessage("Failed to authenticate");
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenEmailAndPasswordAreNull_ShouldThrowArgumentException()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((User)null);

        var act = () => _sessionService.GetValidUserOrThrow(null, "password");

        act.Should().Throw<ArgumentException>().WithMessage("Failed to authenticate");
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenEmailIsIncorrect_ShouldThrowArgumentException()
    {
        var expectedUser = new User()
        {
            Email = "test@test.com",
            Name = "John",
            Lastname = "Lennon",
            Password = "1232524SASADaa!!"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((User)null);

        var act = () => _sessionService.GetValidUserOrThrow("wrong@test.com", expectedUser.Password);

        act.Should().Throw<ArgumentException>().WithMessage("Failed to authenticate");
    }

    [TestMethod]
    public void GetValidUserOrThrow_WhenPasswordIsIncorrect_ShouldThrowArgumentException()
    {
        var expectedUser = new User()
        {
            Email = "test@test.com",
            Name = "John",
            Lastname = "Lennon",
            Password = "1232524SASADaa!!"
        };

        _userRepositoryMock
            .Setup(repo => repo.GetOrDefault(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(expectedUser);

        var act = () => _sessionService.GetValidUserOrThrow(expectedUser.Email, "wrong password");

        act.Should().Throw<ArgumentException>().WithMessage("Failed to authenticate");
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }
    #endregion
}
