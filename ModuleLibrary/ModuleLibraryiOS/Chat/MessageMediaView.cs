using Foundation;
using System;
using UIKit;
using ObjCRuntime;
using MediaPlayer;
using AVFoundation;
using ModuleLibraryiOS.Camera;

namespace ModuleLibraryiOS.Chat
{
    public partial class MessageMediaView : UIView
    {
        public MessageMediaView (IntPtr handle) : base (handle)
        {
        }

        public static MessageMediaView Create(NSData imgData, NSUrl vidUrl, UIViewController vc)
		{
			var arr = NSBundle.MainBundle.LoadNib("MessageMediaView", null, null);
			var v = Runtime.GetNSObject<MessageMediaView>(arr.ValueAt(0));
            v.Start(imgData, vidUrl, vc);
			return v;
		}

		public void Start(NSData imageData, NSUrl videoUrl, UIViewController vc)
		{
			if (imageData != null) showImage(imageData);
			else if (videoUrl != null) showVideo(videoUrl, vc);
		}

		async void showImage(NSData imageData)
		{
			VideoButton.Hidden = true;
			ImageView.Hidden = false;
			var image = new UIImage(imageData);
			var scale = ImageView.Bounds.Width / image.Size.Width;
			ImageView.Image = new UIImage(image.AsJPEG(0.0f), scale);
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            HeightConstraint.Constant = image.Size.Height * scale * 0.7f;
		}

		void showVideo(NSUrl url, UIViewController vc)
		{
			var image = MediaFunctions.GetVideoThumpnail(url);
			ImageView.Hidden = false;
			//var image = new UIImage(imageData);
			var scale = ImageView.Bounds.Width / image.Size.Width;
			ImageView.Image = new UIImage(image.AsJPEG(0.0f), scale);
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			HeightConstraint.Constant = image.Size.Height * scale * 0.7f;

            InspectElementButton.TouchUpInside += (sender, e) => {
                MediaFunctions.PlayClip(url,vc);
            };

            /*
			ImageView.Hidden = true;
			VideoView.Hidden = false;
			HeightConstraint.Constant = 500;

			var moviePlayer = new MPMoviePlayerController(url);
			moviePlayer.View.Frame = new CoreGraphics.CGRect(0, 0, 230, 460);
			moviePlayer.View.SizeToFit();
			//moviePlayer.ScalingMode = MPMovieScalingMode.Fill;
			//moviePlayer.Fullscreen = true;
			moviePlayer.ControlStyle = MPMovieControlStyle.None;
			moviePlayer.SourceType = MPMovieSourceType.File;
			moviePlayer.RepeatMode = MPMovieRepeatMode.One;
			moviePlayer.Play();
			VideoView.AddSubview(moviePlayer.View);*/
		}
    }
}