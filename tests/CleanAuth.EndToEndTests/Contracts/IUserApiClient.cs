using CleanAuth.Application.UseCases.Authentication.Common;
using Refit;

namespace CleanAuth.e2eTests.Contracts;

public interface IUserApiClient : IAuthenticationApiClient
{
    [Get("/api/v1/users/{id}/profile")]
    Task<ApiResponse<AuthResult>> GetProfileAsync([Header("Authorization")] string token, Guid id);
}
