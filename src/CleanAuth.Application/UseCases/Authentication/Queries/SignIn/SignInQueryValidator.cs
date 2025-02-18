using FluentValidation;

namespace CleanAuth.Application.UseCases.Authentication.Queries.SignIn;

public class SignInQueryValidator : AbstractValidator<SignInQuery>
{
    public SignInQueryValidator()
    {        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("is required")
            .EmailAddress()
            .WithMessage("is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("is required")
            .MinimumLength(8)
            .WithMessage("must be at least 8 characters long");
    }
}