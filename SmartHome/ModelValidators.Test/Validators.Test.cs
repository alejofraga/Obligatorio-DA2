using FluentAssertions;
using ModeloValidador.Abstracciones;
using ModelValidatorLength;
using ModelValidatorLettersAndNumbers;

namespace SmartHome.ModelValidators.Test;
[TestClass]
public class Validators_Test
{
    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateValidatorLettersAndNumbers()
    {
        var validator = new ValidatorLettersAndNumbers();

        validator.Should().NotBeNull();
    }

    [TestMethod]
    public void EsValido_WhenStringLengthIsDifferentThanSix_ShouldReturnFalse()
    {
        var validator = new ValidatorLettersAndNumbers();
        var modelo = new Modelo("12345");

        var result = validator.EsValido(modelo);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void EsValido_WhenStringLetterCountIsDifferentThanThree_ShouldReturnFalse()
    {
        var validator = new ValidatorLettersAndNumbers();
        var modelo = new Modelo("ABCDEF");

        var result = validator.EsValido(modelo);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void EsValido_WhenStringDigitCountIsDifferentThanThree_ShouldReturnFalse()
    {
        var validator = new ValidatorLettersAndNumbers();
        var modelo = new Modelo("123456");

        var result = validator.EsValido(modelo);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void Create_WhenInfoIsCorrect_ShouldCreateLengthValidator()
    {
        var validator = new ValidatorLength();

        validator.Should().NotBeNull();
    }

    [TestMethod]
    public void EsValido_WhenLengthIsDifferentThanSix_ShouldReturnFalse()
    {
        var validator = new ValidatorLength();
        var modelo = new Modelo("12345");

        var result = validator.EsValido(modelo);

        result.Should().BeFalse();
    }
}
