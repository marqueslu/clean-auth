using MediatR;

namespace MyMoney.Application.UseCases.User.Queries.Profile;

public record GetProfileQuery(Guid Id) : IRequest<GetProfileResult>
{
    
}