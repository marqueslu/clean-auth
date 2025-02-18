using CleanAuth.Application.UseCases.Authentication.Common;

using MediatR;

namespace CleanAuth.Application.UseCases.Authentication.Commands.SignUp;

public record SignUpCommand(string Name, string Email, string Password) : IRequest<AuthResult>;