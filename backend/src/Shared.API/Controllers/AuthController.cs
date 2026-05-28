using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Auth;
using Shared.Application.Auth.Login;
using Shared.Application.Auth.Logout;
using Shared.Application.Auth.Me;
using Shared.Application.Auth.Refresh;
using Shared.Application.Auth.Register;

namespace Shared.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(
                new RegisterCommand(request.Email, request.Password, request.DisplayName), ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(Problem(result.ErrorMessage, statusCode: 400));
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails(
                ex.Errors.GroupBy(e => e.PropertyName)
                         .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new LoginCommand(request.Email, request.Password), ct);
            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(new ProblemDetails { Title = result.ErrorMessage, Status = 401 });
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails(
                ex.Errors.GroupBy(e => e.PropertyName)
                         .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshCommand(request.RefreshToken), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : Unauthorized(new ProblemDetails { Title = result.ErrorMessage, Status = 401 });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct)
    {
        await mediator.Send(new LogoutCommand(request.RefreshToken), ct);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var result = await mediator.Send(new GetCurrentUserQuery(userId), ct);
        return result is null ? NotFound() : Ok(result);
    }
}

// Request DTOs (thin wrappers — command types live in Application)
public record RegisterRequest(string Email, string Password, string DisplayName);
public record LoginRequest(string Email, string Password);
public record RefreshRequest(string RefreshToken);
public record LogoutRequest(string RefreshToken);
