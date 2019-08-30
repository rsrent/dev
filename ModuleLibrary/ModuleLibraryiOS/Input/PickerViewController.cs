using Foundation;
using System;
using UIKit;

namespace ModuleLibraryiOS.Input
{
    /*
    public partial class PickerViewController : IPickerViewController
    {
        public PickerViewController (IntPtr handle) : base (handle)
        {
        }

		public static PickerController<PickerViewController> Start(UIViewController vc)
		{
			return new PickerController<PickerViewController>(vc, "Input", "PickerViewController");
		}

		public override UILabel GetTitleLabel()
		{
			return TitleLabel;
		}

		public override UIButton GetDoneButton()
		{
			return DoneButton;
		}

		public override UIPickerView GetPicker()
		{
			return Picker;
		}

        public override string[][] GetPickerOptions()
        {
            return null;
        }

        public override void ViewDidLoadPrepersation()
        {
        }

        //Action<string[]> DoneAction;
        //      string InputTitle;
        //      string[][] Items;
        //      Action<UIViewController> Preperation;

        //      public static UIViewController Start(UIView container, UIViewController viewController, string[][] items, string title, Action<string[]> doneAction, Action<UIViewController> prep = null)
        //{
        //	var chatStoryboard = UIStoryboard.FromName("Input", null);
        //	var newView = chatStoryboard.InstantiateViewController("PickerViewController") as PickerViewController;
        //	if (container != null && viewController != null)
        //	{
        //		newView.View.Frame = container.Bounds;
        //		newView.WillMoveToParentViewController(viewController);
        //		container.AddSubview(newView.View);
        //		viewController.AddChildViewController(newView);
        //		newView.DidMoveToParentViewController(viewController);
        //	}
        //	else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
        //	else viewController.PresentViewController(newView, true, null);
        //          newView.ParseInfo(items, title, doneAction, prep);
        //          return newView;
        //}

        //private void ParseInfo(string[][] items, string title, Action<string[]> doneAction, Action<UIViewController> prep = null)
        //{
        //          Items = items;
        //          InputTitle = title;
        //	DoneAction = doneAction;
        //          Preperation = prep;
        //}

        //public override void ViewDidLoad()
        //{
        //	base.ViewDidLoad();
        //	TitleLabel.Text = InputTitle;
        //	DoneButton.TouchUpInside += (sender, e) => {
        //              string[] items = new string[Items.Length];
        //              for (var i = 0; i < Items.Length; i++){
        //                  items[i] = Items[i][Picker.SelectedRowInComponent(i)];
        //              } 
        //              DoneAction.Invoke(items);
        //	};
        //          Picker.Model = new PickerModel(Items);
        //          if (Preperation != null) Preperation.Invoke(this);
        //}



        //      public class PickerModel : UIPickerViewModel
        //{
        //          string[][] Items;

        //          public PickerModel(string[][] items) {
        //              Items = items;
        //          }

        //          public override nint GetComponentCount(UIPickerView picker)
        //	{
        //              return Items.Length;
        //	}

        //	public override nint GetRowsInComponent(UIPickerView picker, nint component)
        //	{
        //              return Items[component].Length;
        //	}

        //	public override string GetTitle(UIPickerView picker, nint row, nint component)
        //	{
        //              return Items[component][row];
        //	}
        //}
    }
*/
}