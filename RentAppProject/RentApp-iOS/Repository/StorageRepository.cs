using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibrary.Shared.Storage;
using ModuleLibraryiOS.Image;
using RentApp.Shared.Models.Document;
using RentApp.Shared.Repositories;
using UIKit;

namespace RentApp.Repository
{
    public class StorageRepository
    {
        private readonly AzureStorage _storage;
        private readonly DocumentRepository _documentRepository;
        private readonly ILocalStorage<byte[]> _localStorage;
        private readonly AzureCredentials _azureCredentials;

        Dictionary<string, string> UploadConnectionStrings = new Dictionary<string, string>();
        Dictionary<string, string> DownloadConnectionStrings = new Dictionary<string, string>();

        Dictionary<string, byte[]> TempStorage = new Dictionary<string, byte[]>();

        ConcurrentDictionary<string, bool> KeysGetting = new ConcurrentDictionary<string, bool>();

        public StorageRepository(AzureStorage storage, DocumentRepository documentRepository, ILocalStorage<byte[]> localStorage, AzureCredentials azureCredentials)
        {
            _storage = storage;
            _documentRepository = documentRepository;
			_localStorage = localStorage;
            _azureCredentials = azureCredentials;
        }

        public async Task<string> Upload(string file, string container) 
        {
            return await _storage.Upload(file, container);
        }

        public async Task<string> Upload(Stream file, string container, string name = null)
        {
            return await _storage.Upload(file, container, name);
        }

        public async Task<byte[]> Download(string name, string container)
        {
            return await _storage.Download(name, container);
        }

        public async void DownloadImageArray(Action<byte[]> loaded, string name, int size = 300)
        => loaded(await DownloadImageArray(name, size));

        public async void DownloadImage(Action<UIImage> loaded, string name, int size = 300)
        {
            if (string.IsNullOrEmpty(name))
                return;
            loaded(new UIImage(NSData.FromArray(await DownloadImageArray(name, size))));
        }

        public async Task<byte[]> DownloadImageArray(string name, int size = 300)
        {
            var container = "";
            if (size <= 50) container = "thumbnail-50";
            else if (size <= 150) container = "thumbnail-150";
            else if (size <= 300) container = "thumbnail-300";
            else if (size <= 500) container = "thumbnail-500";
            else container = "image";

            //var image = await DownloadWithStorage(name, container);
            var image = await Download(name, container);
            if (image != null)
                return image;

            return UIImage.FromFile("imagePlaceholder.png").AsJPEG().ToArray();
        }

        public async Task<UIImage> DownloadImage(string name, int size = 0)
        {
            var array = await DownloadImageArray(name, size);

            return new UIImage(NSData.FromArray(array));
            /*
            if(size > 0) {
                return new UIImage(NSData.FromArray(array.ResizeImage(size)));
            } else {
                return new UIImage(NSData.FromArray(array));
            } */
        }

        public async Task<byte[]> DownloadWithStorage(string name, string container)
        {
            var value = _localStorage.Get(name + container);
            if (value == null)
            {
                if (TempStorage.TryGetValue(name + container, out var storedArray))
                {
                    value =  storedArray;
                } else {
					value = await _storage.Download(name, container);
                }
                _localStorage.Put(name + container, value);
            }
            return value;
        }

        public async Task<byte[]> DownloadWithTemporaryStorage(string name, string container)
        {
            byte[] value = null;
            if (TempStorage.TryGetValue(name + container, out var storedArray))
            {
                return storedArray;
            } else {
                value = await _storage.Download(name, container);
                TempStorage.Add(name + container, value);
            }
            return value;
        }

        public async Task<List<DocumentItem>> GetFiles(string container) {
            try{
                return (await _storage.GetFiles(container)).Select(d => new DocumentItem
                {
                    Title = d.Item1,
                    Uri = d.Item2
                }).ToList();
            } catch {
                return new List<DocumentItem>();
            }
        }
    }
}
