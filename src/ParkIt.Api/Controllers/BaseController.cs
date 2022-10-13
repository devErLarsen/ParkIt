using Microsoft.AspNetCore.Mvc;

namespace ParkIt.Api.Controllers;
[ApiController]
[Route("")]
public abstract class BaseController : ControllerBase
{
    public BaseController() { }
}