using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;
using BMS_Test_Client.Files;

namespace BMS_Test_Client
{
    public class DocumentClient
    {
        private readonly ClientController _client;
        AzureStorage azureStorage;

        public DocumentClient(ClientController client)
        {
            _client = client;

            var documentRepo = new DocumentRepository(_client);
            var azureCridentials = new MyAzureCredentials(documentRepo);
            azureStorage = new AzureStorage(azureCridentials);
        }

        public async Task Run()
        {
            await FixFolder(null);

        }

        private async Task FixFolder(long? folderId)
        {
            if (folderId != null)
                await DownloadContainer(folderId.ToString() + "-folder");

            List<Dictionary<string, object>> childFolders;
            if (folderId == null)
            {
                childFolders = (await _client.GetMany("Document/GetRoots")).ToList();
            }
            else
            {
                childFolders = (await _client.GetMany("Document/GetOfFolder/" + folderId)).ToList();
            }



            if (childFolders.Count == 0) return;
            for (int i = 0; i < childFolders.Count; i++)
            {
                var doc = childFolders[i];
                Console.WriteLine("\n\n\nFolder: ");
                Console.WriteLine(doc["id"]);
                Console.WriteLine(doc["title"]);

                await FixFolder((long)doc["id"]);
            }
            Console.WriteLine("Completed " + childFolders.Count + " child projects of " + folderId + "-folder");
        }

        private async Task DownloadContainer(string containerName)
        {
            try
            {
                var files = await azureStorage.GetFiles(containerName);
                if (files.Count == 0) return;
                Directory.CreateDirectory("Blobs/" + containerName);
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    Console.WriteLine("File: " + file.Item1);
                    //var res = await azureStorage.Download(file.Item1, containerName);
                    //File.WriteAllBytes("Blobs/" + containerName + "/" + file.Item1, res);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
