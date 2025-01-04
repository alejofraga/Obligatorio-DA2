using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Companies;

public interface ICompanyService
{
    Company Add(CreateCompanyArgs createCompanyArgs);
    List<Company> GetCompaniesWithFilters(GetCompaniesArgs getCompaniesArgs);
    List<string> GetValidators();
    bool UserAlreadyOwnsAcompany(string email);
}
