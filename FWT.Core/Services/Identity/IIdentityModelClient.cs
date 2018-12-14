using System.Threading.Tasks;
using IdentityModel.Client;
using OpenTl.Schema;

namespace FWTL.Core.Services.Identity
{
    public interface IIdentityModelClient
    {
        Task<TokenResponse> RequestClientCredentialsTokenAsync(TUser user);
    }
}
