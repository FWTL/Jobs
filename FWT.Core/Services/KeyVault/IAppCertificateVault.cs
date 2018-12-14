using System.Threading.Tasks;

namespace FWTL.Core.Services.KeyVault
{
    public interface IAppCertificateVault
    {
        Task CreateSelfSignedAsync();
    }
}
