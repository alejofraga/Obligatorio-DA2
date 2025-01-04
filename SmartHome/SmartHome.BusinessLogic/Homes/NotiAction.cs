namespace SmartHome.BusinessLogic.Homes;

public class NotiAction
{
    public required Guid NotificationId { get; set; }
    public Notification Notification { get; set; } = null!;
    public bool IsRead { get; set; }
    public required Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public Guid HomeId { get; set; }
    public Home Home { get; set; } = null!;
}
