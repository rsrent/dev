using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Image
{
    public static class Image
    {
        public static UIColor FromHex(this UIColor color, string hexValue, float alpha = 1.0f)
        {
            if (hexValue == null)
                hexValue = "AAAAAA";
            var colorString = hexValue.Replace("#", "");
            if (alpha > 1.0f)
            {
                alpha = 1.0f;
            }
            else if (alpha < 0.0f)
            {
                alpha = 0.0f;
            }

            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                    {
                        red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }
                case 6: // #RRGGBB
                    {
                        red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));

            }
        }

        public static async void Round(this UIImageView ImageView) {
            await Task.Delay(10);
            var min = Math.Min(ImageView.Bounds.Size.Width, ImageView.Bounds.Size.Height);
            ImageView.Layer.CornerRadius = (nfloat)(min / 2.0);
            ImageView.Layer.MasksToBounds = false;


            //ImageView.BackgroundColor = ImageView.Layer.CoFillColor.ToUIColor();
            ImageView.ClipsToBounds = true;

            //var borderThickness = ((CircleImage)Element).BorderThickness;
            var externalBorder = new CALayer();
            externalBorder.CornerRadius = ImageView.Layer.CornerRadius;
            externalBorder.Frame = new CGRect(-.5, -.5, min + 1, min + 1);
            //externalBorder.BorderColor = ((CircleImage)Element).BorderColor.ToCGColor();
            //externalBorder.BorderWidth = ((CircleImage)Element).BorderThickness;

            ImageView.Layer.AddSublayer(externalBorder);
        }

        public static void SetColor(this UIImageView ImageView, UIColor color) {
            ImageView.Image = ImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageView.TintColor = color;
        }

        public static NSData ResizeImage(this NSData data, float width)
        {
            var sourceImage = UIImage.LoadFromData(data);

            //float width = (float)sourceImage.Size.Width / 6f;
            float height = (float)sourceImage.Size.Height / ((float)sourceImage.Size.Width / width);


            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage.AsJPEG();
        }

        public static byte[] ResizeImage(this byte[] img, float width)
        {
            var data = NSData.FromArray(img);
            var sourceImage = UIImage.LoadFromData(data);

            //float width = (float)sourceImage.Size.Width / 6f;
            float height = (float)sourceImage.Size.Height / ((float)sourceImage.Size.Width / width);


            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage.AsJPEG().ToArray();
        }
    }
}
