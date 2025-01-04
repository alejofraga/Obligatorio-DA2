using System.ComponentModel.DataAnnotations;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Homes;

namespace SmartHome.BusinessLogic.Devices;

public class Device
{
    private string _name = null!;
    private string _description = null!;
    private string _modelNumber = null!;
    private List<string> _photos = null!;
    private string? _companyRUT;
    private string _deviceTypeName = null!;

    public required string? DeviceTypeName
    {
        get => _deviceTypeName;
        set
        {
            AssertIsNotEmpty(value, "DeviceTypeName", "Device type");
            AssertDeviceTypeIsValid(value!);
            _deviceTypeName = value!;
        }
    }

    [Key]
    public required string? ModelNumber
    {
        get => _modelNumber;
        set
        {
            AssertIsNotEmpty(value, "ModelNumber", "Model number");
            _modelNumber = value!;
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

    public required string? Description
    {
        get => _description;
        set
        {
            AssertIsNotEmpty(value, "Description", "Description");
            _description = value!;
        }
    }

    public required List<string>? Photos
    {
        get => _photos;
        set
        {
            AssertListIsNotEmpty(value, "Photos", "Photos");
            _photos = value!;
        }
    }

    public required string? CompanyRUT
    {
        get => _companyRUT;
        set
        {
            AssertIsNotEmpty(value, "CompanyRUT", "Company RUT number");
            _companyRUT = value!;
        }
    }

    public Company? Company { get; set; }
    public List<Hardware>? Hardwares { get; set; }

    private static void AssertDeviceTypeIsValid(string value)
    {
        if (!Enum.IsDefined(typeof(ValidDeviceTypes), value!))
        {
            throw new ArgumentException($"Invalid device type: {value}");
        }
    }

    private static void AssertIsNotEmpty(string? value, string attributeName, string attributeAlias)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(attributeName, $"{attributeAlias} cannot be empty");
        }
    }

    private static void AssertListIsNotEmpty(List<string>? value, string attributeName, string attributeAlias)
    {
        if (value == null || value.Count == 0)
        {
            throw new ArgumentNullException(attributeName, $"{attributeAlias} cannot be empty");
        }
    }
}
