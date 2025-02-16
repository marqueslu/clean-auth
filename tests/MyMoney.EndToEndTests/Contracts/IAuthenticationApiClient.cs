using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Application.UseCases.Authentication.Queries.SignIn;
using Refit;

namespace MyMoney.e2eTests.Contracts;

public interface IAuthenticationApiClient
{
    [Post("/api/v1/auth/sign-up")]
    Task<ApiResponse<AuthResult>> SignUpAsync([Body] SignUpCommand command);

    [Post("/api/v1/auth/sign-in")]
    Task<ApiResponse<AuthResult>> SignInAsync([Body] SignInQuery command);
}
