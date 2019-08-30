using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Input;

namespace RentApp
{
    public partial class SimplePickerVC : IPickerViewController
    {
        public SimplePickerVC (IntPtr handle) : base (handle) { }

        string title;
        Action<int> doneAction;
        string[] items;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            PickerController<SimplePickerVC>.Start(this);
            TitelLabel.Text = title;
            DoneButton.TouchUpInside += (sender, e) => {
                this.NavigationController.PopViewController(true);
                doneAction((int) Picker.SelectedRowInComponent(0));
            };
        }

        public override UIPickerView GetPicker()
        => Picker;

        public override string[][] GetPickerOptions()
        => new[] { items };

        public void Setup(string title, string[] items, Action<int> doneAction)
        {
            this.title = title;
            this.items = items;
			this.doneAction = doneAction;
        }
    }
}