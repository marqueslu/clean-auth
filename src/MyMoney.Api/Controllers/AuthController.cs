using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyMoney.Api.ApiModels.Response;
using MyMoney.Application.UseCases.Authentication.Commands.SignUp;
using MyMoney.Application.UseCases.Authentication.Common;
using MyMoney.Application.UseCases.Authentication.Queries.SignIn;

namespace MyMoney.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ApiController
{
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] SignUpCommand request,
        CancellationToken cancellationToken
    )
    {
        var output = await mediator.Send(request, cancellationToken);
        return Ok(output);
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInQuery request,
        CancellationToken cancellationToken
    )
    {
        var output = await mediator.Send(request, cancellationToken);
        return Ok(output);
    }
}