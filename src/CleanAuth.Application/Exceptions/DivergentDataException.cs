namespace CleanAuth.Application.Exceptions;

public class DivergentDataException(string message) : ApplicationException(message)
{
    public static void ThrowIfNoValid(bool @valid, string exceptionMessage)
    {
        if (@valid is false)
            throw new DivergentDataException(exceptionMessage);
    }
}