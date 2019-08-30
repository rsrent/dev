using System;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Input
{
    //public class PickerController<T> : IStarter<T> where T : IPickerViewController
    public class PickerController<T> where T : IPickerViewController
    {
		Action<string[]> DoneAction;
		string InputTitle;
		string[][] Items;
		Action<IPickerViewController> Preperation;

        //public PickerController(IntPtr handle) : base(handle) { }
        //public PickerController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }

        T viewController;

        public static PickerController<T> Start(T vc) {
            return new PickerController<T>(vc);
        }

        public PickerController(T vc) {
            viewController = vc;
            Start();
        }

        /*
        public PickerController<T> Initialize(string[][] items, string title, Action<string[]> doneAction, Action<IPickerViewController> prep = null)
		{
			Items = items;
			InputTitle = title;
			DoneAction = doneAction;
			Preperation = prep;

            return this;
		}*/

        public void Start()
        {
            if(viewController.GetTitleLabel() != null)viewController.GetTitleLabel().Text = InputTitle;
            if (viewController.GetDoneButton() != null) viewController.GetDoneButton().TouchUpInside += (sender, e) => {
				string[] items = new string[Items.Length];
				for (var i = 0; i < Items.Length; i++)
				{
                    items[i] = Items[i][viewController.GetPicker().SelectedRowInComponent(i)];
				}
				DoneAction.Invoke(items);
			};
            if(Items != null)viewController.GetPicker().Model = new PickerModel(Items);
            else viewController.GetPicker().Model = new PickerModel(viewController.GetPickerOptions());

            //viewController.ViewDidLoadPrepersation();

			if (Preperation != null) Preperation.Invoke(viewController);
		}

		public class PickerModel : UIPickerViewModel
		{
			string[][] Items;

			public PickerModel(string[][] items)
			{
				Items = items;
			}

			public override nint GetComponentCount(UIPickerView picker)
			{
				return Items.Length;
			}

			public override nint GetRowsInComponent(UIPickerView picker, nint component)
			{
				return Items[component].Length;
			}

			public override string GetTitle(UIPickerView picker, nint row, nint component)
			{
				return Items[component][row];
			}
		}
    }

	public abstract class IPickerViewController : UIViewController
	{
		public IPickerViewController(IntPtr handle) : base(handle) { }
        public virtual UILabel GetTitleLabel() { return null; }
        public virtual UIButton GetDoneButton() { return null; }
		public abstract UIPickerView GetPicker();
        public abstract string[][] GetPickerOptions();
        //public abstract void ViewDidLoadPrepersation();
	}
}
