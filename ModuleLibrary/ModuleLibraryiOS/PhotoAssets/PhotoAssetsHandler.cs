using System;
using System.Collections.Generic;
using System.Drawing;
using Photos;
using Foundation;
using UIKit;
using AVFoundation;
using System.Threading.Tasks;

namespace ModuleLibraryiOS.PhotoAssets
{
    public class PhotoAssetsHandler
    {
        static Action<NSData> imageSelected;
		static Action<NSUrl> videoSelected;

        //static UIViewController vcSender;
		static UIImagePickerController imagePicker;
		public static void importVideo(UIViewController sender, Action<NSData> img, Action<NSUrl> vid)
		{
			//vcSender = sender;
            imageSelected = img;
            videoSelected = vid;
			imagePicker = new UIImagePickerController();
			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
			imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			imagePicker.Canceled += Handle_Canceled;
			sender.PresentModalViewController(imagePicker, true);
		}

		static void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			// determine what was selected, video or image
			bool isImage = false;
			switch (e.Info[UIImagePickerController.MediaType].ToString())
			{
				case "public.image":
					Console.WriteLine("Image selected");
					isImage = true;
					break;
				case "public.video":
					Console.WriteLine("Video selected");
					break;
			}

			// get common info (shared between images and video)
			NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
			if (referenceURL != null)
				Console.WriteLine("Url:" + referenceURL.ToString());

			// if it was an image, get the other image info
			if (isImage)
			{
				// get the original image
				UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
				if (originalImage != null)
				{

                    imageSelected.Invoke(originalImage.AsJPEG());
					// do something with the image
					Console.WriteLine("got the original image");
					//imageView.Image = originalImage; // display
				}
			}
			else
			{ // if it's a video
			  // get video url
				NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
				if (mediaURL != null)
				{
                    videoSelected.Invoke(mediaURL);
					//saveVideoToAlbum(mediaURL, vcSender);
				}
			}
			// dismiss the picker
			imagePicker.DismissModalViewController(true);
		}

		static void Handle_Canceled(object sender, EventArgs e)
		{
			imagePicker.DismissModalViewController(true);
		}
    }
}
