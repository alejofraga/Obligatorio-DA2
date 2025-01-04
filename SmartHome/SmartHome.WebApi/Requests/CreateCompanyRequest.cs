namespace SmartHome.WebApi.Requests;

public class CreateCompanyRequest
{
    public string? Name { get; set; }

    public string? Rut { get; set; }

    public string? LogoUrl { get; set; }

    public string? Validator { get; set; }
}
