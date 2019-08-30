using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.Storage;
using RentApp.Shared.Repositories;
using RentAppProject;
using UIKit;

namespace RentApp.Repository
{
    public class iOSImageRepository
    {
        ImageRepository _imageRepository;
        StorageRepository _storage;

        public iOSImageRepository(ImageRepository imageRepository, StorageRepository storage)
        {
            _imageRepository = imageRepository;
            _storage = storage;
        }

        public async Task<Dictionary<int, object>> GetUserImages(ICollection<User> users)
        {
            return await _imageRepository.GetUserImages(users, async (ImageLocation) => {
                return await _storage.DownloadImage(ImageLocation);
            });
        }
    }
}
