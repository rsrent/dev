using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Rent.Helpers;

namespace Rent.Repositories
{
    public class AzureStorageReporTobyTest
    {
        private readonly AzureCredentials _credentials;

        public AzureStorageReporTobyTest(AzureCredentials credentials)
        {
            _credentials = credentials;
        }

        // THE ACTUAL ONES:
        public async Task<byte[]> GetFileAsync(string containerName, string name)
        {
            var container = GetContainer(containerName);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public async Task<string> UploadBlobToContainer(string filePath, string container)
        {
            return await UploadFileAsync(container, filePath);
        }

        public async Task<string> UploadBlobToContainer(Stream fileStream, string container)
        {
            return await UploadFileAsync(container, fileStream);
        }

        async Task<string> UploadFileAsync(string containerType, Stream stream)
        {
            var container = GetContainer(containerType.ToLower());
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;
        }

        async Task<string> UploadFileAsync(string containerType, string filePath)
        {
            var container = GetContainer(containerType.ToLower());
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromFileAsync(filePath);

            return name;
        }

        CloudBlobContainer GetContainer(string containerType)
        {
            var account = CloudStorageAccount.Parse(_credentials.ConnectionString);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(containerType.ToLower());
        }
    }
}
