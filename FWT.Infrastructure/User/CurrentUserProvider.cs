using System.Linq;
using System.Security.Claims;
using FWTL.Core.Services.User;

namespace FWTL.Infrastructure.User
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public string PhoneHashId(ClaimsPrincipal user)
        {
            return user.Claims.First(c => c.Type == "PhoneHashId").Value;
        }
    }
}
