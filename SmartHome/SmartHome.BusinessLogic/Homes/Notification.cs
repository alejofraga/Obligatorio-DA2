using System.ComponentModel.DataAnnotations;

namespace SmartHome.BusinessLogic.Homes;

public class Notification()
{
    [Key]
    public Guid Id { get; private init; } = Guid.NewGuid();

    public required string Message { get; set; }

    public DateTime Date { get; init; } = DateTime.Now;

    public Guid HardwareId { get; set; }

    public Hardware Hardware { get; set; } = null!;

    public List<NotiAction> NotiActions { get; set; } = [];

    public Notification(DateTime date)
        : this()
    {
        Date = date;
    }
}
