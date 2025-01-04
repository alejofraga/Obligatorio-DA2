using System.Reflection;
using ModeloValidador.Abstracciones;

namespace SmartHome.BusinessLogic.Devices;

public class ModelValidator : IModelValidator
{
    public bool IsValid(string model, string modelValidatorType)
    {
        var validator = GetValidator(modelValidatorType);

        return validator.EsValido(new Modelo(model));
    }

    private IModeloValidador GetValidator(string modelValidatorType)
    {
        const string folderName = "Validators";
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
        const string searchPattern = "*.dll";
        var dllFiles = Directory.GetFiles(path, searchPattern);

        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToArray();

        var validatorType = assemblies
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => typeof(IModeloValidador).IsAssignableFrom(t) && t.Name == modelValidatorType);
        if (validatorType == null)
        {
            throw new InvalidOperationException("Validator not found");
        }

        var validator = (IModeloValidador?)Activator.CreateInstance(validatorType);
        if (validator == null)
        {
            throw new InvalidOperationException("caould not create instance of validator");
        }

        return validator;
    }
}
