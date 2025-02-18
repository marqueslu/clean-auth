using FluentValidation;

namespace MyMoney.Application.UseCases.User.Queries.Profile;

public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
{
    public GetProfileQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("is required");
    }
}