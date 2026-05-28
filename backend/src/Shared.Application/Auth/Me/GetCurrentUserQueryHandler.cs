using MediatR;
using Shared.Application.Auth;
using Shared.Application.Services;

namespace Shared.Application.Auth.Me;

public class GetCurrentUserQueryHandler(IIdentityService identityService)
    : IRequestHandler<GetCurrentUserQuery, UserInfoDto?>
{
    public async Task<UserInfoDto?> Handle(GetCurrentUserQuery query, CancellationToken ct)
    {
        var user = await identityService.FindByIdAsync(query.UserId);
        if (user is null) return null;

        var roles = await identityService.GetRolesAsync(user.Id);
        return new UserInfoDto(user.Id, user.Email, user.DisplayName, roles.ToList().AsReadOnly());
    }
}
