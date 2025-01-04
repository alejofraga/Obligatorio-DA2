using SmartHome.BusinessLogic.Companies;

namespace SmartHome.WebApi.Responses;

public readonly struct CreateCompanyResponse(Company company)
{
    public readonly string RUT { get; init; } = company.RUT!;

    public readonly string Name { get; init; } = company.Name!;

    public readonly string LogoUrl { get; init; } = company.LogoUrl!;

    public readonly string OwnerEmail { get; init; } = company.OwnerEmail!;

    public readonly string Validator { get; init; } = company.ValidatorTypeName!;
}
