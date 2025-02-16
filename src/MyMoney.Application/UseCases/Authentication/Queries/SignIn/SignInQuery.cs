using MediatR;
using MyMoney.Application.UseCases.Authentication.Common;

namespace MyMoney.Application.UseCases.Authentication.Queries.SignIn;

public record SignInQuery(string Email, string Password) : IRequest<AuthResult>;
