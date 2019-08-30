using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Date;

namespace RentApp
{
    public partial class SimpleDatePickerVC : UIViewController
    {
        public SimpleDatePickerVC (IntPtr handle) : base (handle)
        {
        }

        string title;
        Action<DateTime> doneAction;
        Action<SimpleDatePickerVC> pickerPrep;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TitelLabel.Text = title;
            pickerPrep(this);
            DoneButton.TouchUpInside += (sender, e) =>
            {
                this.NavigationController.PopViewController(true);
                doneAction(DatePicker.Date.NSDateToDateTime());
            };
        }

        public void Setup(string title, Action<SimpleDatePickerVC> pickerPrep, Action<DateTime> doneAction)
        {
            this.title = title;
            this.pickerPrep = pickerPrep;
            this.doneAction = doneAction;
        }
    }
}