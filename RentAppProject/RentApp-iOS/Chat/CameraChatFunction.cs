using System;
using ModuleLibraryiOS.Camera;
using ModuleLibraryiOS.Chat;
using ModuleLibraryiOS.Storage;
using RentAppProject;
using UIKit;
using Microsoft.Extensions.DependencyInjection;
using RentApp.Repository;

namespace RentApp.Chat
{
    public class CameraChatFunction : SpecialChatFunction<Message>
    {
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        UIViewController VC;
        Action<Message> Post;
        public CameraChatFunction(UIViewController vc, Action<Message> post)
        {
            VC = vc;
            Post = post;
        }

        public override string Title() => "Camera";
        public override UIColor Color() => UIColor.Red;

        public override void Clicked()
        {
            CameraContainerViewController.Start(null, VC, async (array) =>
            {
                var imageMessage = new RentMessage.Image();
                var imageStream = array.AsStream();
                try {
                    imageMessage.ImageLocator = await _storage.Upload(imageStream, "image");
                    Post(imageMessage);
                } catch  {
                    
                }


            }, null);
        }
    }
}
