using MediatR;

namespace Shared.Application.Auth.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;
