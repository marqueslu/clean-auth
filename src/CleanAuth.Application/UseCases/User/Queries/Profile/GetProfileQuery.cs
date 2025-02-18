using MediatR;

namespace CleanAuth.Application.UseCases.User.Queries.Profile;

public record GetProfileQuery(Guid Id) : IRequest<GetProfileResult>
{
    
}