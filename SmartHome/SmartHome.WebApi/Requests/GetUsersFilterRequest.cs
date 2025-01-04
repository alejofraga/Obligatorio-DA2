using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Requests;

public class GetUsersFilterRequest
{
    public string? Role { get; set; }

    public string? Fullname { get; set; }

    public int Offset { get; set; } = PaginationDefaultValues.OffSetDefault;

    public int Limit { get; set; } = PaginationDefaultValues.LimitDefault;
}
