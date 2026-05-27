using Microsoft.AspNetCore.Mvc;

namespace Shop.API.Controllers;

[ApiController]
[Route("api/shop")]
public class ShopController : ControllerBase
{
    /// <summary>Health/routing probe for the Shop module.</summary>
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("pong");
}
