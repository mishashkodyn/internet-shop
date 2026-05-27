using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    /// <summary>Health/routing probe for the Blog module.</summary>
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("pong");
}
