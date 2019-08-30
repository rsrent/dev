using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Rent.Helpers;

namespace Rent.Repositories
{
    public class AzureStorageRepository : IStorage
    {
        private readonly AzureCredentials _credentials;

        public AzureStorageRepository(AzureCredentials credentials)
        {
            _credentials = credentials;
        }

        public async Task<string> UploadImage(string filePath)
        {
            return await UploadFileAsync("image", filePath);
        }

        public async Task<string> UploadImage(Stream stream)
        {
            return await UploadFileAsync("image", stream);
        }

        public async Task<byte[]> GetImage(string location)
        {
            return await GetFileAsync("image", location);
        }

        public async Task<string> Upload(string containerName, string filePath)
        {
            return await UploadFileAsync(containerName, filePath);
        }

        public async Task<byte[]> Get(string containerName, string location)
        {
            return await GetFileAsync(containerName, location);
        }
        /*
        public string GetSecureToken() {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_credentials.ConnectionString);

            // Create a new access policy for the account.
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write | SharedAccessAccountPermissions.List,
                Services = SharedAccessAccountServices.Blob | SharedAccessAccountServices.File,
                ResourceTypes = SharedAccessAccountResourceTypes.Service,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            // Return the SAS token.
            return storageAccount.GetSharedAccessSignature(policy);
        }*/

        async Task<IList<string>> GetFilesListAsync(string containerName)
        {
            var container = GetContainer(containerName);

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        async Task<byte[]> GetFileAsync(string containerName, string name)
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

        async Task<string> UploadFileAsync(string containerName, Stream stream)
        {
            var container = GetContainer(containerName);
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;
        }

        async Task<string> UploadFileAsync(string containerName, string filePath)
        {
            var container = GetContainer(containerName);
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromFileAsync(filePath);

            return name;
        }

        async Task<bool> DeleteFileAsync(string containerName, string name)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            var container = GetContainer(containerName);
            return await container.DeleteIfExistsAsync();
        }

        public async Task<bool> CreateContainer(string name)
        {
            string ConnectionString = _credentials.ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(name);
            return await blobContainer.CreateIfNotExistsAsync();
        }

        CloudBlobContainer GetContainer(string containerName)
        {
            var account = CloudStorageAccount.Parse(_credentials.ConnectionString);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(containerName);
        }

        public async Task<string> GetSASTokenUpload(string containerName)
        {
            await CreateContainer(containerName);
            // To create the account SAS, you need to use your shared key credentials. Modify for your account.
            string ConnectionString = _credentials.ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);


            // Create a new access policy for the account.
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read |
                SharedAccessBlobPermissions.Write |
                SharedAccessBlobPermissions.Create |
                SharedAccessBlobPermissions.Delete |
                SharedAccessBlobPermissions.List,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2)
            };

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container.GetSharedAccessSignature(policy);

            // Return the SAS token.
            //return storageAccount.GetSharedAccessSignature(policy);
        }

        public async Task<string> GetContainerSas(string containerName, bool delete = false)
        {
            await CreateContainer(containerName);
            var container = GetContainer(containerName);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read;
            if (delete)
            {
                sasConstraints.Permissions |= SharedAccessBlobPermissions.Delete;
            }

            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return sasContainerToken;
        }

        public string GetBlobSasUri(string containerName, string blobName, string policyName = null)
        {
            string sasBlobToken;
            var container = GetContainer(containerName);

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                // Create a new access policy and define its constraints.
                // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and
                // to construct a shared access policy that is saved to the container's shared access policies.
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
                };

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

                Console.WriteLine("SAS for blob (ad hoc): {0}", sasBlobToken);
                Console.WriteLine();
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

                Console.WriteLine("SAS for blob (stored access policy): {0}", sasBlobToken);
                Console.WriteLine();
            }

            // Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

    }
}
