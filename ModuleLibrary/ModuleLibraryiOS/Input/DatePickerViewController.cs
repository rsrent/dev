using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using System.Threading.Tasks;

namespace ModuleLibraryiOS.Input
{
    public partial class DatePickerViewController : IDatePickerViewController
    {

        public DatePickerViewController(IntPtr handle) : base(handle)
        {
        }

		public static DatePickerController<DatePickerViewController> Start(UIViewController vc)
		{
			return new DatePickerController<DatePickerViewController>(vc, "Input", "DatePickerViewController");
		}

		public override UILabel GetTitleLabel()
		{
			return TitleLabel;
		}

		public override UIButton GetDoneButton()
		{
			return DoneButton;
		}

		public override UIDatePicker GetDatePicker()
		{
			return DatePicker;
		}
    }
}