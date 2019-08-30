using Foundation;
using System;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public partial class UITextViewPlaceholder : UITextView
    {
        public UITextViewPlaceholder (IntPtr handle) : base (handle)
        {
        }

        public static void SetUpTextField(UITextViewPlaceholder TextField, string Placeholder) {
            TextField.Layer.BorderColor = new UIColor(0.60f, 0.60f, 0.60f, 1f).CGColor;
			TextField.Layer.BorderWidth = 0.5f;
            TextField.Layer.CornerRadius = 8f;
			TextField.TextColor = new UIColor(0.60f, 0.60f, 0.60f, 1f);

			TextField.Text = Placeholder;
			TextField.ShouldBeginEditing = t =>
			{
				if (TextField.Text == Placeholder)
				{
					TextField.TextColor = UIColor.Black;
					TextField.Text = string.Empty;
				}
				return true;
			};
			TextField.ShouldEndEditing = t =>
			{
				//TextField.ResignFirstResponder();
				if (string.IsNullOrEmpty(TextField.Text))
				{
					TextField.TextColor = new UIColor(0.60f, 0.60f, 0.60f, 1f);
					TextField.Text = Placeholder;
				}
				return true;
			};

            /*
			TextField.ShouldChangeText += (textView, range, text) => {
				if (text == "\n")
				{
					textView.ResignFirstResponder();
					return false;
				}
				return true;
			}; */
        }
    }
}