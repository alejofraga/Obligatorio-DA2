using FluentAssertions;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Devices;

[TestClass]
public class Device_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateDevice()
    {
        const string expectedModelNumber = "1";
        const string expectedName = "Camera";
        const string expectedDescription = "This is a camera";
        var expectedPhotos = new List<string>() { "www.camera.com" };
        var expectedCompany = GetValidCompany();

        var newDevice = new Device()
        {
            ModelNumber = expectedModelNumber,
            Name = expectedName,
            Description = expectedDescription,
            Photos = expectedPhotos,
            CompanyRUT = expectedCompany.RUT,
            Company = expectedCompany,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };

        newDevice.Should().NotBeNull();
        newDevice.ModelNumber.Should().Be(expectedModelNumber);
        newDevice.Name.Should().Be(expectedName);
        newDevice.Description.Should().Be(expectedDescription);
        newDevice.Photos.Should().BeEquivalentTo(expectedPhotos);
        newDevice.CompanyRUT.Should().Be(expectedCompany.RUT);
        newDevice.Company.Should().BeEquivalentTo(expectedCompany);
    }

    [TestMethod]
    [DataRow("A156", "Camera", "", "800450300128", "Description", "Description")]
    [DataRow("A156", "Camera", "    ", "800450300128", "Description", "Description")]
    [DataRow("A156", "Camera", null, "800450300128", "Description", "Description")]
    [DataRow("A156", "", "HD 4K", "800450300128", "Name", "Name")]
    [DataRow("A156", "    ", "HD 4K", "800450300128", "Name", "Name")]
    [DataRow("A156", null, "HD 4K", "800450300128", "Name", "Name")]
    [DataRow("", "Camera", "HD 4K", "800450300128", "Model number", "ModelNumber")]
    [DataRow("    ", "Camera", "HD 4K", "800450300128", "Model number", "ModelNumber")]
    [DataRow(null, "Camera", "HD 4K", "800450300128", "Model number", "ModelNumber")]
    [DataRow("A156", "Camera", "HD 4K", "", "Company RUT number", "CompanyRUT")]
    [DataRow("A156", "Camera", "HD 4K", "    ", "Company RUT number", "CompanyRUT")]
    [DataRow("A156", "Camera", "HD 4K", null, "Company RUT number", "CompanyRUT")]
    public void Create_WhenRequiredInfoIsEmpty_ShouldThrowException(string modelNumber, string name, string description, string companyRUT,
        string attributeAlias, string attributeName)
    {
        var act = () => new Device()
        {
            ModelNumber = modelNumber,
            Name = name,
            Description = description,
            Photos = ["www.camera.com"],
            CompanyRUT = companyRUT,
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString()
        };

        act.Should().Throw<ArgumentNullException>().WithMessage($"{attributeAlias} cannot be empty (Parameter '{attributeName}')");
    }

    [TestMethod]
    public void Create_WhenPhotosListIsEmpty_ShouldThrowException()
    {
        var act = () => new Device()
        {
            ModelNumber = "A156",
            Name = "Camera",
            Description = "HD 4K",
            CompanyRUT = GetValidCompany().RUT,
            Photos = [],
            DeviceTypeName = ValidDeviceTypes.Sensor.ToString(),
        };

        act.Should().Throw<ArgumentNullException>().WithMessage("Photos cannot be empty (Parameter 'Photos')");
    }

    [TestMethod]
    public void Create_WhenDeviceTypeIsInvalid_ShouldThrow()
    {
        var act = () => new Device()
        {
            DeviceTypeName = "invalid",
            CompanyRUT = "1",
            Photos = [],
            ModelNumber = "1",
            Name = "name",
            Description = "description"
        };

        act.Should().Throw<ArgumentException>();
    }

    #region SampleData
    private User GetValidUser()
    {
        return new User()
        {
            Email = "maticor93@gmail.com",
            Name = "Matias",
            Lastname = "Corvetto",
            Password = "#Adf123456"
        };
    }

    private Company GetValidCompany()
    {
        const string validatorType = "ValidatorLength";

        return new Company(validatorType)
        {
            Name = "CamerasSA",
            RUT = "800450300128",
            LogoUrl = "www.cameraSA.com",
            OwnerEmail = GetValidUser().Email
        };
    }
    #endregion
}
