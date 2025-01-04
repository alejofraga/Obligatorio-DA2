using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Users;
using SmartHome.WebApi.Controllers;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Test;

[TestClass]
public class MeNotificationController_Test
{
    private static Mock<IHomeService> _homeServiceMock = null!;
    private static Mock<HttpContext> _httpContextMock = null!;
    private MeNotificationController _memberNotificationController = null!;

    [TestInitialize]
    public void OnInitialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        _memberNotificationController =
            new MeNotificationController(_homeServiceMock.Object);
    }

    [TestCleanup]
    public void OnCleanup()
    {
        _homeServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void GetNotifications_WhenAllFiltersAreValid_ShouldReturnOkResponse()
    {
        var memberId = Guid.NewGuid();
        var userLogged = GetValidUser();
        var member = GetValidMember(userLogged);
        var dateTime = DateTime.Now;
        var notification = GetValidNotification(dateTime, member);
        var notiAction = GetValidNotiAction(memberId, notification);
        var filter = new GetNotificationsFilterRequest();
        _memberNotificationController.ControllerContext =
            new ControllerContext { HttpContext = _httpContextMock.Object };

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(userLogged);
        _homeServiceMock
            .Setup(hs =>
                hs.GetUserNotificationsWithFilters(It.IsAny<GetUserNotificationsArgs>()))
            .Returns([notiAction]);

        var act = _memberNotificationController.GetNotifications(filter);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Data")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        var responseList = responseValue as List<GetNotificationsResponse>;
        responseList.Should().NotBeNull();
        responseList!.Count.Should().Be(1);
        var notiResponse = responseList!.First();
        notiResponse.Should().NotBeNull();
        notiResponse.DateTime.Should().Be(dateTime.ToString("g"));
        notiResponse.Message.Should().Be("message");
        notiResponse.DeviceType.Should().Be("Sensor");
        notiResponse.State.Should().BeTrue();
        notiResponse.HardwareId.Should().Be(notiAction.Notification.HardwareId.ToString());
    }

    [TestMethod]
    public void ReadNotifications_WhenAllFiltersAreValid_ShouldReturnOkResponse()
    {
        var memberId = Guid.NewGuid();
        var userLogged = GetValidUser();
        var member = GetValidMember(userLogged);
        var dateTime = DateTime.Now;
        var notification = GetValidNotification(dateTime, member);
        var notiAction = GetValidNotiAction(memberId, notification);
        var readNotificationsRequest = new ReadNotificationsRequest { NotificationsIds = [notification.Id] };
        _memberNotificationController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

        _httpContextMock
            .Setup(h => h.Items[Item.UserLogged]).Returns(userLogged);
        _homeServiceMock
            .Setup(hs => hs.ReadNotifications(It.IsAny<List<Guid>>(), It.IsAny<User>()));

        var act = _memberNotificationController.ReadNotifications(readNotificationsRequest);

        var result = act as ObjectResult;
        result.Should().NotBeNull();
        var responseValue = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value);
        responseValue.Should().NotBeNull();
        responseValue.Should().Be("Notifications readed!");
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User
        {
            Name = "Alejo",
            Lastname = "Fraga",
            Password = "As1231@weqewqss",
            Email = "alejofraga22v2@gmail.com"
        };
    }

    private Member GetValidMember(User userLogged)
    {
        return new Member
        {
            UserEmail = userLogged.Email,
            HomeId = Guid.NewGuid()
        };
    }

    private Notification GetValidNotification(DateTime dateTime, Member member)
    {
        var device = new Device()
        {
            ModelNumber = "A2b",
            Name = "camera",
            Description = "HD",
            Photos = ["photo"],
            CompanyRUT = "123456789012",
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };

        var hardware = new Hardware
        {
            HomeId = member.HomeId,
            Device = device,
            DeviceModelNumber = device.ModelNumber
        };

        return new Notification(dateTime)
        {
            Message = "message",
            Hardware = hardware
        };
    }

    private NotiAction GetValidNotiAction(Guid memberId, Notification notification)
    {
        return new NotiAction
        {
            MemberId = memberId,
            IsRead = true,
            Notification = notification,
            NotificationId = notification.Id
        };
    }
    #endregion
}
