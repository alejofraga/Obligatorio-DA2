using FluentAssertions;
using SmartHome.BusinessLogic.Devices;

namespace SmartHome.BusinessLogic.Test.Devices;

[TestClass]
public class ModelValidator_Test
{
    private ModelValidator _modelValidator = null!;

    [TestInitialize]
    public void Setup()
    {
        _modelValidator = new ModelValidator();
    }

    [TestMethod]
    public void IsValid_WhenValidatoFolderIsNotFound_ShouldThrow()
    {
        var model = "model";
        var modelValidatorType = "invalidModelValidatorType";

        var act = () => _modelValidator.IsValid(model, modelValidatorType);

        act.Should().Throw<DirectoryNotFoundException>();
    }

    [TestMethod]
    public void IsValid_WhenValidatorTypeIsNotFound_ShouldThrow()
    {
        var model = "model";
        const string validatorsFolder = "Validators";
        var modelValidatorType = "invalidModelValidatorType";
        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, validatorsFolder));

        var act = () => _modelValidator.IsValid(model, modelValidatorType);

        act.Should().Throw<InvalidOperationException>();
        Directory.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, validatorsFolder), true);
    }
}
