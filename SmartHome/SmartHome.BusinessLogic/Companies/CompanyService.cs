using System.Reflection;
using ModeloValidador.Abstracciones;
using SmartHome.BusinessLogic.Args;

namespace SmartHome.BusinessLogic.Companies;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    public Company Add(CreateCompanyArgs createCompanyArgs)
    {
        var company = new Company(createCompanyArgs.Validator!)
        {
            Name = createCompanyArgs!.Name,
            RUT = createCompanyArgs.RUT,
            LogoUrl = createCompanyArgs.LogoUrl,
            OwnerEmail = createCompanyArgs.OwnerEmail!
        };

        if (UserAlreadyOwnsAcompany(createCompanyArgs.OwnerEmail!))
        {
            throw new InvalidOperationException("User already owns a company");
        }

        if (companyRepository.Exist(c => c.RUT == createCompanyArgs.RUT))
        {
            throw new InvalidOperationException("RUT number already in use");
        }

        companyRepository.Add(company);
        return company;
    }

    public List<Company> GetCompaniesWithFilters(GetCompaniesArgs getCompaniesArgs)
    {
        var companies = companyRepository.GetCompaniesWithFilters(getCompaniesArgs);
        return companies;
    }

    public bool UserAlreadyOwnsAcompany(string email)
    {
        return companyRepository.Exist(c => c.OwnerEmail.ToUpper() == email.ToUpper());
    }

    public List<string> GetValidators()
    {
        const string folderName = "Validators";
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        const string searchPattern = "*.dll";
        var dllFiles = Directory.GetFiles(path, searchPattern);

        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToArray();

        var validators = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModeloValidador).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => t.Name)
            .ToList();

        return validators;
    }
}
