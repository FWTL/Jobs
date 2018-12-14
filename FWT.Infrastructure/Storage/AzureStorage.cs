using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWTL.Core.Extensions;
using FWTL.Core.Helpers;
using FWTL.Core.Services.Storage;
using FWTL.Infrastructure.Schema;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NodaTime;

namespace FWTL.Infrastructure.Storage
{
    public class AzureStorage : IStorage
    {
        private readonly CloudBlobClient _blobClient;

        private readonly CloudStorageAccount _storageAccount;

        private IClock _clock;

        public AzureStorage(AzureStorageCredentials credentials, IClock clock)
        {
            _storageAccount = CloudStorageAccount.Parse(credentials.ConnectionString);
            _blobClient = _storageAccount.CreateCloudBlobClient();
            _clock = clock;
        }

        public async Task<bool> ExistsAsync(string containerName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            return await container.ExistsAsync().ConfigureAwait(false);
        }

        public string GetAccountSasUri(TimeSpan expireAfter)
        {
            SharedAccessAccountPolicy adHocPolicy = new SharedAccessAccountPolicy()
            {
                SharedAccessExpiryTime = _clock.UtcNow().Add(expireAfter),
                Protocols = SharedAccessProtocol.HttpsOnly,
                SharedAccessStartTime = DateTime.Now,
                Services = SharedAccessAccountServices.Blob,
                Permissions = SharedAccessAccountPermissions.Read,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
            };

            return _storageAccount.GetSharedAccessSignature(adHocPolicy);
        }

        public async Task<List<BlobFileInfo>> ListAsync(string containerName)
        {
            var container = _blobClient.GetContainerReference(containerName);

            BlobContinuationToken continuationToken = null;
            List<CloudBlockBlob> results = new List<CloudBlockBlob>();
            do
            {
                var response = await container.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.None, null, continuationToken, null, null).ConfigureAwait(false);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results.Cast<CloudBlockBlob>());
            }
            while (continuationToken != null);

            return results.Select(file => new BlobFileInfo()
            {
                Name = file.Name,
                Uri = file.Uri
            }).ToList();
        }

        public async Task UploadAsync(Guid containerId, List<FileInfo> files)
        {
            var container = _blobClient.GetContainerReference(containerId.ToString("n"));
            await container.CreateAsync().ConfigureAwait(false);

            foreach (FileInfo file in files)
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.Name);
                await blockBlob.UploadFromByteArrayAsync(file.Content, 0, file.Content.Length).ConfigureAwait(false);
            }
        }
    }
}
