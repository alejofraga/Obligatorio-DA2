using SmartHome.BusinessLogic.Homes;

namespace SmartHome.WebApi.Responses;

public readonly struct GetNotificationsResponse(NotiAction notiAction)
{
    public readonly string Message { get; init; } = notiAction.Notification.Message;

    public readonly string HardwareId { get; init; } = notiAction.Notification.HardwareId.ToString();

    public readonly bool State { get; init; } = notiAction.IsRead;

    public readonly string DateTime { get; init; } = notiAction.Notification.Date.ToString("g");

    public readonly string DeviceType { get; init; } = notiAction.Notification.Hardware.Device!.DeviceTypeName!;

    public readonly string HomeId { get; init; } = notiAction.HomeId.ToString();

    public readonly string NotificationId { get; init; } = notiAction.NotificationId.ToString();
}
