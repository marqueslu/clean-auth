using MyMoney.Application.Exceptions;
using MyMoney.Application.UseCases.Common;
using MyMoney.Domain.Repository;

namespace MyMoney.Application.UseCases.User.Queries.Profile;

public class GetProfileQueryHandler(IUserRepository userRepository) : IQueryHandler<GetProfileQuery, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.Id, cancellationToken);
        NotFoundException.ThrowIfNull(user, "User not found.");
        return new GetProfileResult(user!.Id, user.Name, user.Email);
    }
}