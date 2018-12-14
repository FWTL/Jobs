using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FWTL.Infrastructure.Configuration
{
    public class AzureSecretsVaultProvider : ConfigurationProvider
    {
        private readonly string _baseUrl;

        private readonly string _clientId;

        private readonly string _clientSecret;

        public AzureSecretsVaultProvider(string baseUrl, string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _baseUrl = baseUrl;
        }

        public override void Load()
        {
            var parser = new AzureSecretParser(_baseUrl, _clientId, _clientSecret);
            Task<IDictionary<string, string>> t = parser.ParseAsync();
            t.Wait();
            Data = t.Result;
        }
    }
}
