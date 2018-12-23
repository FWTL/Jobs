using System.Security.Claims;

namespace FWTL.Core.Services.User
{
    public interface ICurrentUserProvider
    {
        string UserId(ClaimsPrincipal user);
    }
}
