using System.ComponentModel.DataAnnotations;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Homes;

public sealed class Home()
{
    private int _memberCount;

    public required string OwnerEmail { get; set; }
    public User? Owner { get; set; }
    public required Coordinates Coordinates { get; set; }
    public required Location Location { get; set; }

    public List<Room> Rooms { get; set; } = [];
    public string? Name { get; set; }
    public required int MemberCount
    {
        get => _memberCount!;

        set
        {
            AssertIsGreaterThanZero(value);
            _memberCount = value;
        }
    }

    [Key]
    public Guid Id { get; private init; } = Guid.NewGuid();

    public List<Member> Members { get; set; } = [];

    public List<Hardware> Hardwares { get; set; } = [];

    public Home(Guid id)
        : this()
    {
        Id = id;
    }

    private static void AssertIsGreaterThanZero(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(MemberCount), "Member count must be greater than 0");
        }
    }
}
