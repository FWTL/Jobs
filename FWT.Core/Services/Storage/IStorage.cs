using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FWTL.Core.Helpers;

namespace FWTL.Core.Services.Storage
{
    public interface IStorage
    {
        Task<bool> ExistsAsync(string containerName);

        string GetAccountSasUri(TimeSpan expireAfter);

        Task<List<BlobFileInfo>> ListAsync(string containerName);

        Task UploadAsync(Guid containerId, List<FileInfo> files);
    }
}
