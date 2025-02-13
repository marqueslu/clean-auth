using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyMoney.Api.Controllers;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
}