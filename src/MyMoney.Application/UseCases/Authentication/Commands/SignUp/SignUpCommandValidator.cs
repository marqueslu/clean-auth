using FluentValidation;

namespace MyMoney.Application.UseCases.Authentication.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("is required")
            .MinimumLength(5)
            .WithMessage("must be at least 5 characters long")
            .MaximumLength(100)
            .WithMessage("must be a maximum of 100 characters long");

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