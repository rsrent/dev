using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class CleaningPlanTasksCustomerCell : UITableViewCell
    {
        public CleaningPlanTasksCustomerCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(CleaningTask task) {
            DescriptionLabel.Text = task.Comment;

            DescriptionLabel.Text = task.Comment;
            DescriptionLabel.TextColor = UIColor.Black;
            if (string.IsNullOrEmpty(task.Comment))
            {
                DescriptionLabel.Text = task.Area.Description;
                DescriptionLabel.TextColor = UIColor.FromName("FadeTextColor");
            }

            if (task.SquareMeters > 0)
                DescriptionLabel.Text += ", " + task.SquareMeters + "m²";

            if (task.Frequency != null)
            {
                TimeLabel.Text = task.Frequency.ToString();
            } else if (task.TimesOfYear != null) {
                TimeLabel.Text = task.TimesOfYear + " gange årligt";
            }
            else {
                TimeLabel.Text = "";
            }
        }
    }
}