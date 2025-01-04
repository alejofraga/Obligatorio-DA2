namespace SmartHome.BusinessLogic.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}
