namespace CleanAuth.Application.Exceptions;

public class NotFoundException(string message) : ApplicationException(message)
{
    public static void ThrowIfNull(object? @object, string exceptionMessage)
    {
        if (@object is null)
            throw new NotFoundException(exceptionMessage);
    }
}
