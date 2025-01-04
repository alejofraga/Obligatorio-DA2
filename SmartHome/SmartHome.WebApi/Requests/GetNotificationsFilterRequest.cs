namespace SmartHome.WebApi.Requests;

public class GetNotificationsFilterRequest
{
    public string? DeviceType { get; set; }

    public string? DateTime { get; set; }

    public bool? Read { get; set; }
}
