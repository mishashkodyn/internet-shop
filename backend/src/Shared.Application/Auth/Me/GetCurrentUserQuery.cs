using MediatR;
using Shared.Application.Auth;

namespace Shared.Application.Auth.Me;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserInfoDto?>;
