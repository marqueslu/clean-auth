namespace CleanAuth.Application.Common.Behaviors.Validation;

public record ValidationError(string PropertyName, string ErrorMessage);