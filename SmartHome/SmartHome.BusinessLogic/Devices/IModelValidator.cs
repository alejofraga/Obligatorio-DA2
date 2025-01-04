namespace SmartHome.BusinessLogic.Devices;

public interface IModelValidator
{
    public bool IsValid(string model, string modelValidatorType);
}
