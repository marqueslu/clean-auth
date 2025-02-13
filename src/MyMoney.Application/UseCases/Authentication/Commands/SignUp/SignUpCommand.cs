using MediatR;

using MyMoney.Application.UseCases.Authentication.Common;

namespace MyMoney.Application.UseCases.Authentication.Commands.SignUp;

public record SignUpCommand(string Name, string Email, string Password) : IRequest<AuthResult>;