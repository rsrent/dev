using Foundation;
using System;
using UIKit;
using ObjCRuntime;

namespace ModuleLibraryiOS.Chat
{
    public partial class MessageViewControllerView : UIView
    {
        public MessageViewControllerView (IntPtr handle) : base (handle)
        {
        }

        public static MessageViewControllerView Create(Action<UIView> startViewController)
		{
			var arr = NSBundle.MainBundle.LoadNib("MessageViewControllerView", null, null);
			var v = Runtime.GetNSObject<MessageViewControllerView>(arr.ValueAt(0));
			v.Start(startViewController);
			return v;
		}

		public void Start(Action<UIView> startViewController)
		{
            startViewController.Invoke(ContainerView);

            ContainerViewHeightConstraint.Constant = 300;
		}
    }
}