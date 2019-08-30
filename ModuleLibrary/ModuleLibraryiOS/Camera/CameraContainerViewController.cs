using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;

using MediaPlayer;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.PhotoAssets;
using AVFoundation;

namespace ModuleLibraryiOS.Camera
{
    public partial class CameraContainerViewController : UIViewController
    {
        bool down;
        bool video;
        Dictionary<CameraController.CameraAction, Action> cameraDic;
        MPMoviePlayerController moviePlayer;

        Action<NSData> postAnImage;
        Action<NSUrl> postAVideo;

        NSUrl videoUrl;
        NSData imageArray;

		public static void Start(UIView container, UIViewController viewController, Action<NSData> postImage, Action<NSUrl> postVideo)
		{
			var chatStoryboard = UIStoryboard.FromName("Camera", null);
			var newView = chatStoryboard.InstantiateViewController("CameraContainerViewController") as CameraContainerViewController;
            newView.parseInfo(postImage, postVideo);
            Instanciate.Start(container, viewController, newView);
		} 

        public void parseInfo(Action<NSData> postImage, Action<NSUrl> postVideo)
        {
            this.postAnImage = postImage;
            this.postAVideo = postVideo;
        }

        public CameraContainerViewController EnableImage(Action<NSData> postImage) {
            this.postAnImage = postImage;
            return this;
        }

        public CameraContainerViewController EnableVideo(Action<NSUrl> postVideo)
        {
            this.postAVideo = postVideo;
            return this;
        }

        public CameraContainerViewController (IntPtr handle) : base (handle)
        {
        }

        async void checkIfVideo() {
            await Task.Delay(100);
            if (down && postAVideo != null)
            {
                cameraDic[CameraController.CameraAction.StartRecording].Invoke();
                video = true;
            }
            else video = false;
        }

        public override void ViewWillAppear(bool animated)
        {
            if (NavigationController != null) NavigationController.NavigationBar.Hidden = true;
            if (TabBarController != null) TabBarController.TabBar.Hidden = true;
        }
        public override void ViewWillDisappear(bool animated)
        {
            if (NavigationController != null) NavigationController.NavigationBar.Hidden = false;
            if (TabBarController != null) TabBarController.TabBar.Hidden = false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            cameraDic = CameraController.StartCamera(CameraView, this, NavigationController, videoTaken, pictureTaken);

            TakePictureButton.TouchUpInside += (sender, e) => {
                if(video) cameraDic[CameraController.CameraAction.StopRecording].Invoke();
                else cameraDic[CameraController.CameraAction.TakePicture].Invoke();
                down = false;
            };

            TakePictureButton.TouchDown += (sender, e) => {
                down = true;
                checkIfVideo();
            };

            CancelButton.TouchUpInside += (sender, e) => {
                ImageView.Image = null;
                if(moviePlayer != null){
                    moviePlayer.Stop();
                    moviePlayer.View.RemoveFromSuperview();
                }
                moviePlayer = null;

                videoUrl = null;
                imageArray = null;

                video = false;

                TakePictureButton.Hidden = false;
                ImageView.Hidden = true;
				UseButton.Hidden = true;
				CancelButton.Hidden = true;
                VideoView.Hidden = true;
                SwapButton.Hidden = false;
            };

            if(NavigationController != null) {
				BackButton.TouchUpInside += (sender, e) => {
					NavigationController.PopViewController(true);
				};
            }


            UseButton.TouchUpInside += (sender, e) => {
                if (NavigationController != null) NavigationController.PopViewController(true);
                if (video) postAVideo.Invoke(videoUrl);
                else postAnImage.Invoke(imageArray);
                video = false;
            };

            SwapButton.TouchUpInside += (sender, e) => {
                cameraDic[CameraController.CameraAction.SwitchCamera].Invoke();
            };

            AlbumButton.TouchUpInside += (sender, e) => {
                PhotoAssetsHandler.importVideo(this, pictureTaken, videoTaken);
            };
        }

        private void pictureTaken(NSData array) {
            imageArray = array;

            var image = new UIImage(array);
            ImageView.Image = image;

            TakePictureButton.Hidden = true;
            ImageView.Hidden = false;
            UseButton.Hidden = false;
            CancelButton.Hidden = false;
            SwapButton.Hidden = true;
        }

        private void videoTaken(NSUrl url) {
            videoUrl = url;

            moviePlayer = new MPMoviePlayerController(url);
            moviePlayer.View.Frame = new CoreGraphics.CGRect(0, 0, View.Frame.Size.Width, View.Frame.Size.Height);
            moviePlayer.View.SizeToFit();
            moviePlayer.ScalingMode = MPMovieScalingMode.Fill;
            moviePlayer.Fullscreen = true;
            moviePlayer.ControlStyle = MPMovieControlStyle.None;
            moviePlayer.SourceType = MPMovieSourceType.File;
            moviePlayer.RepeatMode = MPMovieRepeatMode.One;
            moviePlayer.Play();
            VideoView.AddSubview(moviePlayer.View);

            TakePictureButton.Hidden = true;
            VideoView.Hidden = false;
			UseButton.Hidden = false;
			CancelButton.Hidden = false;
			SwapButton.Hidden = true;
        }
    }
}