using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Homes;
using SmartHome.BusinessLogic.Sessions;

namespace SmartHome.BusinessLogic.Users;

public sealed class User
{
    private string _email = null!;
    private string _password = null!;
    private string _name = null!;
    private string _lastname = null!;

    private static readonly Regex _passwordRegex = new Regex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*._?&#])[A-Za-z\d@$!%*._?&#]*$",
        RegexOptions.Compiled);

    private static readonly Regex _emailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    [Key]
    public required string? Email
    {
        get => _email;

        set
        {
            AssertIsNotEmpty(value, "Email");
            IsValidEmail(value!);
            _email = value!;
        }
    }

    public required string? Password
    {
        get => _password;
        set
        {
            AssertIsNotEmpty(value, "Password");
            AssertIsValidPassword(value!);
            _password = value!;
        }
    }

    public required string? Name
    {
        get => _name;
        set
        {
            AssertIsNotEmpty(value, "Name");
            _name = value!;
        }
    }

    public required string? Lastname
    {
        get => _lastname;
        set
        {
            AssertIsNotEmpty(value, "Lastaname");
            _lastname = value!;
        }
    }

    public DateTime AccountCreation { get; init; } = DateTime.Now;
    public string? ProfilePicturePath { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<Member> Members { get; set; } = [];
    public Session Session { get; set; } = null!;

    public bool HasPermission(string? permissionName)
    {
        return Roles.Any(r => r.SystemPermissions.Any(sp => sp.Name.ToUpper() == permissionName.ToUpper()));
    }

    private static void IsValidEmail(string value)
    {
        if (!_emailRegex.IsMatch(value))
        {
            throw new FormatException("Invalid email format");
        }
    }

    private static void AssertIsNotEmpty(string? value, string attibuteName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(attibuteName, $"{attibuteName} cannot be empty");
        }
    }

    private static void AssertIsValidPassword(string value)
    {
        const int PASSWORD_MIN_LENGTH = 8;

        if (value.Length < PASSWORD_MIN_LENGTH)
        {
            throw new FormatException("Password must be at least 8 characters long");
        }

        if (!_passwordRegex.IsMatch(value))
        {
            throw new FormatException("Password must contain at least one uppercase letter, one lowercase letter, " +
                "one number and one special character.");
        }
    }
}
