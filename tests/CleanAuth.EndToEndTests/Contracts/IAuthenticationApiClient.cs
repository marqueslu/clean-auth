using CleanAuth.Application.UseCases.Authentication.Commands.SignUp;
using CleanAuth.Application.UseCases.Authentication.Common;
using CleanAuth.Application.UseCases.Authentication.Queries.SignIn;
using Refit;

namespace CleanAuth.e2eTests.Contracts;

public interface IAuthenticationApiClient
{
    [Post("/api/v1/auth/sign-up")]
    Task<ApiResponse<AuthResult>> SignUpAsync([Body] SignUpCommand command);

    [Post("/api/v1/auth/sign-in")]
    Task<ApiResponse<AuthResult>> SignInAsync([Body] SignInQuery command);
}
