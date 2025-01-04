using ModeloValidador.Abstracciones;

namespace ModelValidatorLettersAndNumbers;

public class ValidatorLettersAndNumbers : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        const int validLength = 6;
        var model = modelo.Value;
        return model.Length == validLength && Enumerable.Count<char>(model, char.IsLetter) == 3 && Enumerable.Count<char>(model, char.IsDigit) == 3;
    }
}
