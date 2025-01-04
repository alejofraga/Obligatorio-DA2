using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Companies;

public interface ICompanyRepository : IRepository<Company>
{
    List<Company> GetCompaniesWithFilters(GetCompaniesArgs getCompaniesArgs);
}
