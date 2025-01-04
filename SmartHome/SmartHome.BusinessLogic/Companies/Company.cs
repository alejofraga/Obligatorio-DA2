using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Companies;

public class Company()
{
    private string _rut = null!;
    private string _name = null!;
    private string? _logoUrl = null!;

    private static readonly Regex _rutRegex = new Regex(@"^\d{12}$",
        RegexOptions.Compiled);

    public string ValidatorTypeName { get; set; } = null!;

    public Company(string validator)
        : this()
    {
        ValidatorTypeName = validator;
    }

    [Key]
    public required string? RUT
    {
        get => _rut;

        set
        {
            AssertIsNotEmpty(value, "RUT", "RUT");
            AssertIsValidRUT(value!);
            _rut = value!;
        }
    }

    public required string? Name
    {
        get => _name;

        set
        {
            AssertIsNotEmpty(value, "Name", "Name");
            _name = value!;
        }
    }

    public required string? LogoUrl
    {
        get => _logoUrl;

        set
        {
            AssertIsNotEmpty(value, "Logo url", "LogoUrl");
            _logoUrl = value;
        }
    }

    public required string OwnerEmail { get; set; }

    public User? Owner { get; set; }

    private static void AssertIsNotEmpty(string? value, string attributeAlias, string attributeName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(attributeName, $"{attributeAlias} cannot be empty");
        }
    }

    private static void AssertIsValidRUT(string value)
    {
        if (!_rutRegex.IsMatch(value))
        {
            throw new FormatException("RUT number must be a sequence of 12 digits");
        }
    }
}
