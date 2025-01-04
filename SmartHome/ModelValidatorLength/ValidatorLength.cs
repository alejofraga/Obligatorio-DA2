using ModeloValidador.Abstracciones;

namespace ModelValidatorLength;

public class ValidatorLength : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        const int validLength = 6;
        var model = modelo.Value;
        return Enumerable.Count<char>(model, char.IsLetter) == validLength;
    }
}
