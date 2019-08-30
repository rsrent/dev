using Foundation;
using System;
using UIKit;

namespace ModuleLibraryiOS.Input
{
    public partial class TextInputViewController : ITextInputViewController
    {
        public TextInputViewController (IntPtr handle) : base (handle)
        {
        }

		public static TextInputController<TextInputViewController> Start(UIViewController vc)
		{
			return new TextInputController<TextInputViewController>(vc, "Input", "TextInputViewController");
		}

		public override UILabel GetTitleLabel()
		{
			return TitleLabel;
		}

		public override UITextField[] GetTextFields()
		{
            return new [] {
                TextField
            };
		}

		public override UIButton GetDoneButton()
		{
			return DoneButton;
		}
		/*
		string InputTitle;
        string Placeholder;
        UIKeyboardType KeyboardType;
		Action<string> DoneAction;
        Action<UIViewController> Preperation;

        public static UIViewController Start(UIView container, UIViewController viewController, string title, string placeholder, UIKeyboardType keyboard, Action<string> doneAction, Action<UIViewController> prep = null)
		{
			var chatStoryboard = UIStoryboard.FromName("Input", null);
			var newView = chatStoryboard.InstantiateViewController("TextInputViewController") as TextInputViewController;
			if (container != null && viewController != null)
			{
				newView.View.Frame = container.Bounds;
				newView.WillMoveToParentViewController(viewController);
				container.AddSubview(newView.View);
				viewController.AddChildViewController(newView);
				newView.DidMoveToParentViewController(viewController);
			}
			else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
            newView.ParseInfo(title, placeholder, keyboard, doneAction, prep);
            return newView;
		}

		private void ParseInfo(string title, string placeholder, UIKeyboardType keyboard, Action<string> doneAction, Action<UIViewController> prep = null)
		{
			InputTitle = title;
            Placeholder = placeholder;
            KeyboardType = keyboard;
			DoneAction = doneAction;
            Preperation = prep;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            if (Preperation != null) Preperation.Invoke(this);

			TitleLabel.Text = InputTitle;
            TextField.Placeholder = Placeholder;
            TextField.KeyboardType = KeyboardType;
            if(KeyboardType == UIKeyboardType.NumberPad) TextField.TextAlignment = UITextAlignment.Center;
			DoneButton.TouchUpInside += (sender, e) => {
                DoneAction.Invoke(TextField.Text);
			};
            TextField.BecomeFirstResponder();
		} */

		
    }
}