using SmartHome.BusinessLogic.Companies;

namespace SmartHome.WebApi.Responses;

public readonly struct GetCompaniesResponse(Company company)
{
    public readonly string CompanyName { get; init; } = company.Name!;

    public readonly string RUT { get; init; } = company.RUT!;

    public readonly string OwnerEmail { get; init; } = company.OwnerEmail!;

    public readonly string OwnerFullName { get; init; } = company.Owner!.Name + " " + company.Owner.Lastname;
}
