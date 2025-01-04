using SmartHome.WebApi.Filters;

namespace SmartHome.WebApi.Requests;

public class GetCompaniesFilterRequest
{
    public string? CompanyName { get; set; }

    public string? OwnerFullname { get; set; }

    public int Offset { get; set; } = PaginationDefaultValues.OffSetDefault;

    public int Limit { get; set; } = PaginationDefaultValues.LimitDefault;
}
