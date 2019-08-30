using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS_Test_Client.Files
{
    public class MyAzureCredentials : AzureCredentials
    {
        private readonly DocumentRepository _documentRepository;

        string connectionString = "https://rentstorage.blob.core.windows.net/";
        string connectionStringDev = "https://rentdevelopmentstorage.blob.core.windows.net/";

        //string connectionString = "https://rentstorage.blob.core.windows.net/";

        Dictionary<string, string> UploadConnectionStrings = new Dictionary<string, string>();
        Dictionary<string, string> DownloadConnectionStrings = new Dictionary<string, string>();

        ConcurrentDictionary<string, bool> KeysGetting = new ConcurrentDictionary<string, bool>();

        bool Live = true;

        public MyAzureCredentials(DocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public override async Task<string> GetDownloadConnectionString(string container)
        {
            var sas = (Live ? connectionString : connectionStringDev) + container + (await GetString(false, container));
            System.Diagnostics.Debug.WriteLine(sas);
            return sas;
        }

        public override async Task<string> GetUploadConnectionString(string container)
        {
            var sas = (Live ? connectionString : connectionStringDev) + container + (await GetString(true, container));
            System.Diagnostics.Debug.WriteLine(sas);
            return sas;
        }

        async Task<string> GetString(bool upload, string container)
        {
            while (KeysGetting.Any(v => v.Key.Equals(upload.ToString() + container)))
            {
                await Task.Delay(100);
            }

            if (upload)
            {
                if (!UploadConnectionStrings.ContainsKey(container))
                {
                    KeysGetting.TryAdd(upload.ToString() + container, upload);

                    await _documentRepository.GetUploadSAS(container, (obj) =>
                    {
                        UploadConnectionStrings.Add(container, obj);
                    });
                    KeysGetting.TryRemove(upload.ToString() + container, out bool garbage);
                }
                await Task.Delay(10);
                if (UploadConnectionStrings.TryGetValue(container, out var connectionString))
                {
                    return connectionString;
                }
            }
            else
            {
                if (!DownloadConnectionStrings.ContainsKey(container))
                {
                    KeysGetting.TryAdd(upload.ToString() + container, upload);

                    await _documentRepository.GetDownloadSAS(container, (obj) =>
                    {
                        DownloadConnectionStrings.Add(container, obj);
                    });
                    KeysGetting.TryRemove(upload.ToString() + container, out bool garbage);
                }
                await Task.Delay(10);
                if (DownloadConnectionStrings.TryGetValue(container, out var connectionString))
                {
                    return connectionString;
                }
            }
            return null;
        }
    }
}
