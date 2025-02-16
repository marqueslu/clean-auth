namespace MyMoney.Application.Exceptions;

public class ConflictException(string message) : ApplicationException(message)
{
    public static void ThrowIfNotNull(object? @object, string exceptionMessage)
    {
        if (@object is not null)
            throw new ConflictException(exceptionMessage);
    }
}
