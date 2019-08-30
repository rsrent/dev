using Foundation;
using System;
using UIKit;

namespace RentApp
{
    public partial class ImageVC : UIViewController
    {
        public ImageVC (IntPtr handle) : base (handle)
        {
        }

        UIImage Image;

        public void ParseInfo(byte[] image) {
            Image = new UIImage(NSData.FromArray(image));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var imageView = new UIImageView(Image);

            ScrollView.ContentSize = imageView.Image.Size;

            System.Diagnostics.Debug.WriteLine(imageView.Image.Size);
            System.Diagnostics.Debug.WriteLine(View.Bounds.Size);

            ScrollView.AddSubview(imageView);

            ScrollView.MaximumZoomScale = 3f;
            ScrollView.MinimumZoomScale = View.Bounds.Size.Width / imageView.Image.Size.Width;
            ScrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return imageView; };

            UITapGestureRecognizer doubletap = new UITapGestureRecognizer(OnDoubleTap)
            {
                NumberOfTapsRequired = 2
            };
            ScrollView.AddGestureRecognizer(doubletap);

            ScrollView.SetZoomScale(View.Bounds.Size.Width / imageView.Image.Size.Width,false);
        }

        private void OnDoubleTap(UIGestureRecognizer gesture)
        {
            if (ScrollView.ZoomScale >= 1)
                ScrollView.SetZoomScale(0.25f, true);
            else
                ScrollView.SetZoomScale(2f, true);
        }
    }
}