using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuth.Api.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
}