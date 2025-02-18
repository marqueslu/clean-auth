using FluentValidation.Results;

namespace CleanAuth.Application.Common.Behaviors.Validation;

public class ValidationException : Exception
{
    public ValidationException(string message) :base(message)
    {
    }
}