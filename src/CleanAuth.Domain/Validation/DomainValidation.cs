using System.Text.RegularExpressions;

using CleanAuth.Domain.Exceptions;

namespace CleanAuth.Domain.Validation;

public abstract partial class DomainValidation
{
    protected DomainValidation() { }

    public static void NotNullAttribute(object? target, string fieldName)
    {
        if (target is null)
            throw new EntityValidationException($"{fieldName} should not be null.");
    }

    public static void NotNullOrEmptyAttribute(string target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
            throw new EntityValidationException($"{fieldName} should not be null or empty.");
    }

    public static void MinLengthAttribute(string target, int minLength, string fieldName)
    {
        if (target.Length < minLength)
            throw new EntityValidationException(
                $"{fieldName} should be at least {minLength} characters long."
            );
    }

    public static void ValidEmailAttribute(string target, string fieldName)
    {
        var emailRegex = EmailRegexPattern();
        if (!emailRegex.IsMatch(target))
            throw new EntityValidationException($"{fieldName} is not a valid email address.");
    }

    public static void MaxLengthAttribute(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
            throw new EntityValidationException(
                $"{fieldName} should be less or equal to {maxLength} characters long."
            );
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegexPattern();

    public static object NotNull(string value, string fieldName)
    {
        throw new NotImplementedException();
    }
}
