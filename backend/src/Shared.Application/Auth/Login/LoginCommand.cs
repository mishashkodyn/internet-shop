using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult<TokenPairDto>>;
