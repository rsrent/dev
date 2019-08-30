using System;
using System.Linq;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Input
{
    public class TextInputController<T> : IStarter<T> where T : ITextInputViewController
    {
		string InputTitle;
		string[] Placeholders;
		UIKeyboardType[] KeyboardTypes;
		Action<string[]> DoneAction;
        Action<ITextInputViewController> Prep;

        public TextInputController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }

        public TextInputController<T> Initialize(Action<string[]> doneAction, string title = null, string[] placeholders = null, UIKeyboardType[] keyboards = null, Action<ITextInputViewController> prep = null)
		{
			InputTitle = title;
			Placeholders = placeholders;
			KeyboardTypes = keyboards;
			DoneAction = doneAction;
            Prep = prep;
            return this;
		}

        public override void ViewDidLoad() {

            if (Prep != null)
                Prep.Invoke(viewController);

            if(viewController.GetTitleLabel() != null && InputTitle != null)
                viewController.GetTitleLabel().Text = InputTitle;

            for (var i = 0; i < viewController.GetTextFields().Length; i++) {
                if(Placeholders != null)viewController.GetTextFields()[i].Placeholder = Placeholders[i];
                if(KeyboardTypes != null)viewController.GetTextFields()[i].KeyboardType = KeyboardTypes[i];

                if (viewController.GetTextFields()[i].KeyboardType == UIKeyboardType.NumberPad) 
                    viewController.GetTextFields()[i].TextAlignment = UITextAlignment.Center;
            }
            viewController.GetDoneButton().TouchUpInside += (sender, e) => {
                DoneAction.Invoke(viewController.GetTextFields().Select(tf => tf.Text).ToArray());
			};
			viewController.GetTextFields()[0].BecomeFirstResponder();
        }
    }

	public abstract class ITextInputViewController : IStartable
	{
        public ITextInputViewController(IntPtr handle) : base(handle) { }
		public abstract UILabel GetTitleLabel();
		public abstract UITextField [] GetTextFields();
		public abstract UIButton GetDoneButton();
	}
}
