namespace MyMoney.Application.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(string message)
        : base(message) { }

    public static void ThrowIfNotNull(object? @object, string exceptionMessage)
    {
        if (@object is not null)
            throw new ConflictException(exceptionMessage);
    }
}
