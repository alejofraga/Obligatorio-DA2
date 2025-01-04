using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Requests;

public class GetHomesFilterRequest()
{
    public int Limit { get; set; } = PaginationDefaultValues.LimitDefault;
    public int Offset { get; set; } = PaginationDefaultValues.OffSetDefault;
}
