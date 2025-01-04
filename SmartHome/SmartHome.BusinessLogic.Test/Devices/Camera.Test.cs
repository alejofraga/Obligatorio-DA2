using FluentAssertions;
using SmartHome.BusinessLogic.Companies;
using SmartHome.BusinessLogic.Devices;
using SmartHome.BusinessLogic.Users;

namespace SmartHome.BusinessLogic.Test.Devices;

[TestClass]
public class Camera_Test
{
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

    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateCamera()
    {
        const string expectedModelNumber = "1";
        const string expectedName = "Camera";
        const string expectedDescription = "This is a camera";
        var expectedPhotos = new List<string>() { "www.camera.com" };
        const bool expectedHasMovementDetection = true;
        const bool expectedPersonDetection = true;
        const bool ExpectedIsOutdoor = false;
        const bool ExpectedIsIndoor = true;

        var newCamera = new Camera(ExpectedIsOutdoor, ExpectedIsIndoor)
        {
            ModelNumber = expectedModelNumber,
            Name = expectedName,
            Description = expectedDescription,
            Photos = expectedPhotos,
            CompanyRUT = GetValidCompany().RUT,
            HasMovementDetection = expectedHasMovementDetection,
            HasPersonDetection = expectedPersonDetection,
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };

        newCamera.Should().NotBeNull();
        newCamera.ModelNumber.Should().Be(expectedModelNumber);
        newCamera.Name.Should().Be(expectedName);
        newCamera.Description.Should().Be(expectedDescription);
        newCamera.Photos.Should().BeEquivalentTo(expectedPhotos);
        newCamera.HasMovementDetection.Should().Be(expectedHasMovementDetection);
        newCamera.HasPersonDetection.Should().Be(expectedPersonDetection);
        newCamera.IsOutdoor.Should().Be(ExpectedIsOutdoor);
        newCamera.IsIndoor.Should().Be(ExpectedIsIndoor);
    }

    [TestMethod]
    public void Create_WhenRequiredInfoIsInconsistent_ShouldThrowException()
    {
        const bool InconsistentIsOutdoor = false;
        const bool InconsistentIsIndoor = false;

        var act = () => new Camera(InconsistentIsOutdoor, InconsistentIsIndoor)
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = GetValidCompany().RUT,
            HasMovementDetection = true,
            HasPersonDetection = true,
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };

        act.Should().Throw<ArgumentException>().WithMessage("A camera must be at least indoor or outdoor");
    }

    [TestMethod]
    public void Create_WhenCompanyHasMovementDetectionIsEmpty_ShouldThrowException()
    {
        var act = () => new Camera()
        {
            ModelNumber = "1",
            Name = "Camera",
            Description = "This is a camera",
            Photos = ["www.camera.com"],
            CompanyRUT = GetValidCompany().RUT,
            HasMovementDetection = null,
            HasPersonDetection = true,
            IsOutdoor = false,
            IsIndoor = true,
            DeviceTypeName = nameof(ValidDeviceTypes.Camera)
        };

        act.Should().Throw<ArgumentNullException>().WithMessage("Movement detection indicator cannot be empty (Parameter 'HasMovementDetection')");
    }

    #region SampleData
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
