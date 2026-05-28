using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthResult<TokenPairDto>>;
