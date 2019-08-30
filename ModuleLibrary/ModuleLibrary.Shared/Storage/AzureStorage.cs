using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;

namespace ModuleLibrary.Shared.Storage
{
    public class AzureStorage : IStorage
    {
        private readonly AzureCredentials _credentials;


        public AzureStorage(AzureCredentials credentials)
        {
            _credentials = credentials;
        }

        /*
        public async Task<byte[]> DownloadWithStorage(string name, string container)
        {
            var value = _localStorage.Get(name + container);
            if (value == null)
            {
                value = await Download(name, container);
                _localStorage.Put(name + container, value);
            }
            return value;
        } */

        public async Task<byte[]> Download(string name, string containerName)
        {
            try {
                var container = await GetContainer(containerName, false);
                //var blob = container.GetBlockBlobReference(name);
                var blob = container.GetBlockBlobReference(name);

                if (await blob.ExistsAsync())
                {
                    await blob.FetchAttributesAsync();
                    byte[] blobBytes = new byte[blob.Properties.Length];
                    await blob.DownloadToByteArrayAsync(blobBytes, 0);
                    return blobBytes;
                }
            } catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
            return null;
        }

        public async Task<string> Upload(string blobPath, string container)
        {
            return await UploadFileAsync(container, blobPath);
        }

        public async Task<string> Upload(Stream blobStream, string container, string name = null)
        {
            return await UploadFileAsync(container, blobStream, name);
        }

        async Task<string> UploadFileAsync(string containerName, Stream stream, string name = null)
        {
            var container = await GetContainer(containerName.ToLower(), true);
            //await container.();
            if(name == null)
                name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;
        }

        async Task<string> UploadFileAsync(string containerName, string filePath)
        {
            var container = await GetContainer(containerName.ToLower(), true);
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromFileAsync(filePath);

            return name;
        }

        public async Task<List<(string, Uri)>> GetFiles(string containerName) {
            var container = await GetContainer(containerName, false);
            var result = await container.ListBlobsSegmentedAsync(null);

            foreach(var blob in result.Results) {
                System.Diagnostics.Debug.WriteLine(((CloudBlob)blob).Name);
            }

            return result.Results
                         .Where(b => !(((CloudBlob)b).Metadata.ContainsKey("delete") 
                                    && ((CloudBlob)b).Metadata["delete"] == "true"))
                         .Select(b => (((CloudBlob)b).Name, ((CloudBlob)b).Uri)).ToList();
        }

        async Task<CloudBlobContainer> GetContainer(string containerName, bool upload)
        {
            CloudBlobContainer container;

            if (upload)
                container = new CloudBlobContainer(new Uri(await _credentials.GetUploadConnectionString(containerName)));
            else 
                container = new CloudBlobContainer(new Uri(await _credentials.GetDownloadConnectionString(containerName)));

            /*
            try {
                await container.ExistsAsync();
            } catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }*/


            return container;
        }
    }
}