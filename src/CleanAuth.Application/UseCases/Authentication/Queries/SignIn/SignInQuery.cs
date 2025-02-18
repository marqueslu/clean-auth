using CleanAuth.Application.UseCases.Authentication.Common;

using MediatR;

namespace CleanAuth.Application.UseCases.Authentication.Queries.SignIn;

public record SignInQuery(string Email, string Password) : IRequest<AuthResult>;
