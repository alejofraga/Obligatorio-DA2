using Microsoft.EntityFrameworkCore;
using SmartHome.BusinessLogic.Args;
using SmartHome.BusinessLogic.Companies;

namespace SmartHome.DataLayer;

public class CompanyRepository(DbContext context) : Repository<Company>(context), ICompanyRepository
{
    private readonly DbSet<Company> _companies = context.Set<Company>();

    public List<Company> GetCompaniesWithFilters(GetCompaniesArgs getCompaniesArgs)
    {
        var query = _companies.Include(c => c.Owner).AsQueryable();

        if (!string.IsNullOrEmpty(getCompaniesArgs.CompanyName))
        {
            query = query.Where(c => c.Name.ToUpper().StartsWith(getCompaniesArgs.CompanyName.ToUpper()));
        }

        if (!string.IsNullOrEmpty(getCompaniesArgs.OwnerFullname))
        {
            var upperFullname = getCompaniesArgs.OwnerFullname.ToUpper();

            query = query.Where(c =>
                (c.Owner.Name + " " + c.Owner.Lastname).ToUpper().StartsWith(upperFullname));
        }

        query = query.Skip(getCompaniesArgs.Offset)
            .Take(getCompaniesArgs.Limit);

        return query.ToList();
    }
}
