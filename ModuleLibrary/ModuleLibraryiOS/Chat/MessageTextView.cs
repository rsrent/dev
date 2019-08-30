using Foundation;
using System;
using UIKit;
using ObjCRuntime;

namespace ModuleLibraryiOS.Chat
{
    public partial class MessageTextView : UIView
    {
        public MessageTextView (IntPtr handle) : base (handle)
        {
        }

		public static MessageTextView Create(string text)
		{
			var arr = NSBundle.MainBundle.LoadNib("MessageTextView", null, null);
			var v = Runtime.GetNSObject<MessageTextView>(arr.ValueAt(0));
            v.Start(text);
			return v;
		}

        public void Start(string text) {
            MessageLabel.Text = text;
        }
    }
}