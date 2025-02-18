using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace CleanAuth.Application.Common.Behaviors.Validation;

public class ValidateBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
            return await next();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {
            return await next();
        }

        throw new ValidationException(FormatValidationErrors(validationResult.Errors));
    }
    
    private static string FormatValidationErrors(List<ValidationFailure> errors)
    {
        return string.Join("\n", errors
            .GroupBy(e => e.PropertyName)
            .Select(g => $"{g.Key}: {string.Join(" and ", g.Select(e => e.ErrorMessage))}"));
    }
}