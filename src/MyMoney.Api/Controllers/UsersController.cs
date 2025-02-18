using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMoney.Api.ApiModels.Response;
using MyMoney.Application.UseCases.User.Queries.Profile;

namespace MyMoney.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator) : ApiController
{
    [HttpGet("{id:guid}/profile")]
    [ProducesResponseType(typeof(ApiResponse<GetProfileResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProfile(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var userIdFromToken = User.FindFirst("Id")?.Value;
        if (userIdFromToken != id.ToString())
            return Forbid();
        var output = await mediator.Send(new GetProfileQuery(id), cancellationToken);
        return Ok(output);
    }
}
