using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Requests;

public class GetDevicesFiltersRequest
{
    public string? Name { get; set; }

    public string? ModelNumber { get; set; }

    public string? CompanyName { get; set; }

    public string? DeviceType { get; set; }

    public int Offset { get; set; } = PaginationDefaultValues.OffSetDefault;

    public int Limit { get; set; } = PaginationDefaultValues.LimitDefault;
}
