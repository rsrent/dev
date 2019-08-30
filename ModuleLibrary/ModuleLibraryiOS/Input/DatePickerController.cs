using System;
using Foundation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Input
{
    public class DatePickerController<T> : IStarter<T> where T : IDatePickerViewController
    {
		string InputTitle;
		Action<DateTime> DoneAction;

		DateTime? MinDate = null;
		DateTime? MaxDate = null;

        public DatePickerController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }

        public UIViewController Initialize(Action<DateTime> doneAction,string title = null, DateTime? minDate = null, DateTime? maxDate = null)
		{
			InputTitle = title;
			DoneAction = doneAction;
			MinDate = minDate;
			MaxDate = maxDate;
            return this;
		}

        public override void ViewDidLoad() {
            if (viewController.GetTitleLabel() != null && InputTitle != null)
			    viewController.GetTitleLabel().Text = InputTitle;
			viewController.GetDoneButton().TouchUpInside += (sender, e) => {
				DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
				DoneAction.Invoke(reference.AddSeconds(viewController.GetDatePicker().Date.SecondsSinceReferenceDate));
			};

			if (MinDate != null) viewController.GetDatePicker().MinimumDate = DateTimeToNSDate((DateTime)MinDate);
			if (MaxDate != null) viewController.GetDatePicker().MinimumDate = DateTimeToNSDate((DateTime)MaxDate);
        }

		public static DateTime NSDateToDateTime(NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate DateTimeToNSDate(DateTime date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - reference).TotalSeconds);
		}
    }

	public abstract class IDatePickerViewController : IStartable
	{
		public IDatePickerViewController(IntPtr handle) : base(handle) { }

		public abstract UILabel GetTitleLabel();
		public abstract UIButton GetDoneButton();
		public abstract UIDatePicker GetDatePicker();
	}
}
