using System.Net;
using Microsoft.AspNetCore.Mvc;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Homes;
using SmartHome.WebApi.Filters;
using SmartHome.WebApi.Requests;
using SmartHome.WebApi.Responses;

namespace SmartHome.WebApi.Controllers;
[ApiController]
[Route("me/notifications")]
[Authentication]
public class MeNotificationController(IHomeService homeService) : SmartHomeControllerBase
{
    [HttpGet]
    public ObjectResult GetNotifications([FromQuery] GetNotificationsFilterRequest? filter)
    {
        filter ??= new GetNotificationsFilterRequest();
        var getUserNotificationsArgs = new GetUserNotificationsArgs()
        {
            Read = filter!.Read,
            DateTime = filter.DateTime,
            DeviceType = filter.DeviceType,
            LoggedUser = GetUserLogged()
        };
        var notificationsList = homeService.GetUserNotificationsWithFilters(getUserNotificationsArgs);

        var getNotificationsResponse = notificationsList.Select(notiAction => new GetNotificationsResponse(notiAction)).ToList();

        return new ObjectResult(new
        {
            Data = getNotificationsResponse
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpPatch]
    public ObjectResult ReadNotifications(ReadNotificationsRequest? readNotificationsRequest)
    {
        homeService.ReadNotifications(readNotificationsRequest.NotificationsIds!, GetUserLogged());

        return new ObjectResult(new
        {
            Message = "Notifications readed!"
        })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
