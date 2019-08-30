using System;

using Foundation;
using MediaPlayer;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public partial class MediaCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MediaCell");
        public static readonly UINib Nib;

        static MediaCell()
        {
            Nib = UINib.FromName("MediaCell", NSBundle.MainBundle);
        }

        protected MediaCell(IntPtr handle) : base(handle)
        {
            Transform = CoreGraphics.CGAffineTransform.MakeRotation(3.14159f);
        }

		public MediaCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
		}

        public void UpdateCell(NSData imageData, NSUrl videoUrl, DateTime time, bool usersMessage, bool showTime)
		{
			TimeLabel.Text = time.ToString("U");
            LeftImageButton.Layer.BorderColor = UIColor.White.CGColor;
            RightImageButton.Layer.BorderColor = UIColor.White.CGColor;


			if (showTime)
			{
				TimeLabelHeightConstraint.Constant = 30;
			}
			else
			{
				TimeLabelHeightConstraint.Constant = 0;
			}
            /*
			if (usersMessage)
			{
				SenderImage.Hidden = true;
				BottomSpaceConstraint.Constant = 0;
				RoundButtonBottomConstraint.Constant = 0;
			}
			else
			{
				SenderImage.Hidden = false;
				BottomSpaceConstraint.Constant = 10;
				RoundButtonBottomConstraint.Constant = 10;
			}*/


			if (usersMessage)
			{
				LeftImage.Hidden = true;
				RightImage.Hidden = false;
			}
			else
			{
				RightImage.Hidden = true;
				LeftImage.Hidden = false;
			}



			if (imageData != null) showImage(imageData);
			else if (videoUrl != null) showVideo(videoUrl);
		}

		void showImage(NSData imageData)
		{
            VideoView.Hidden = true;
            ImageView.Hidden = false;
			var image = new UIImage(imageData);
			var scale = ImageView.Bounds.Width / image.Size.Width;
			ImageView.Image = new UIImage(image.AsJPEG(0.0f), scale);
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			HeightConstraint.Constant = image.Size.Height * scale;
		}

		void showVideo(NSUrl url)
		{
			ImageView.Hidden = true;
            VideoView.Hidden = false;
			HeightConstraint.Constant = 500;

			var moviePlayer = new MPMoviePlayerController(url);
			moviePlayer.View.Frame = new CoreGraphics.CGRect(0, 0, 230, 500);
			moviePlayer.View.SizeToFit();
			//moviePlayer.ScalingMode = MPMovieScalingMode.Fill;
			//moviePlayer.Fullscreen = true;
			moviePlayer.ControlStyle = MPMovieControlStyle.None;
			moviePlayer.SourceType = MPMovieSourceType.File;
            moviePlayer.RepeatMode = MPMovieRepeatMode.One;
			moviePlayer.Play();
			VideoView.AddSubview(moviePlayer.View);
		}
	}
}
