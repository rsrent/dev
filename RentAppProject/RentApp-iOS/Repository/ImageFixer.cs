using System;
using Rent.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using RentAppProject;
using Foundation;

namespace RentApp.Repository
{
    public class ImageFixer
    {


        public ImageFixer()
        {
            





        }


        public static async void TryFixAll()
        {
            
            var userRepo = AppDelegate.ServiceProvider.GetService<UserRepository>();

            userRepo.GetUsers((obj) => {
                FixIt(obj);
            });
        }

        static async void FixIt(ICollection<User> users)
        {

            var storageRepo = AppDelegate.ServiceProvider.GetService<StorageRepository>();

            foreach(var user in users)
            {
                if(user.ImageLocation != null)
                {
                    var img = await storageRepo.Download(user.ImageLocation, "image");

                    var imageStream = NSData.FromArray(img).AsStream();

                    await storageRepo.Upload(imageStream, "image", user.ImageLocation);
                }
            }
        }
    }
}
