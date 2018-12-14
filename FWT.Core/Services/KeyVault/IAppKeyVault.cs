using System.Threading.Tasks;

namespace FWTL.Core.Services.KeyVault
{
    public interface IAppKeyVault
    {
        Task<string> DecryptAsync(string value);

        string Encrypt(string value);
    }
}
