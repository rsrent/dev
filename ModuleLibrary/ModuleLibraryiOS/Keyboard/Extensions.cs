using System;
using CoreGraphics;
using UIKit;

namespace ModuleLibraryiOS.Keyboard
{
    public static class Extensions
    {
        public static void AddOkAndReturn(this UITextView textView, UIView View) 
        {
            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, View.Frame.Size.Width, 44.0f));
            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { textView.ResignFirstResponder(); })
            };
            textView.InputAccessoryView = toolbar;
        }

        public static void AddOkAndReturn(this UITextField textField, UIView View)
        {
            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, View.Frame.Size.Width, 44.0f));
            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { textField.ResignFirstResponder(); })
            };
            textField.InputAccessoryView = toolbar;
        }
    }
}
