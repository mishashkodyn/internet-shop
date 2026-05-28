using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Auth.Register;

public record RegisterCommand(string Email, string Password, string DisplayName)
    : IRequest<AuthResult<TokenPairDto>>;
