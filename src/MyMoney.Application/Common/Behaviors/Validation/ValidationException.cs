using FluentValidation.Results;

namespace MyMoney.Application.Common.Behaviors.Validation;

public class ValidationException : Exception
{
    public ValidationException(string message) :base(message)
    {
    }
}