using Microsoft.Extensions.Configuration;

namespace FWTL.Infrastructure.Configuration
{
    public class AzureSecretsVaultSource : IConfigurationSource
    {
        private readonly string _baseUrl;

        private readonly string _clientId;

        private readonly string _clientSecret;

        public AzureSecretsVaultSource(string baseUrl, string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _baseUrl = baseUrl;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AzureSecretsVaultProvider(_baseUrl, _clientId, _clientSecret);
        }
    }
}
