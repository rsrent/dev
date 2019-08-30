using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class CleaningPlanTasksCell : UITableViewCell
    {
        public CleaningPlanTasksCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(CleaningTask task)
		{
			if (task.Frequency != null)
			{
				FrequenzyLabel.Text = task.Frequency.ToString();
			}
            else if (task.TimesOfYear != null)
			{
                FrequenzyLabel.Text = task.TimesOfYear + " gange årligt";

				//if (task.Interval == 1) FrequenzyLabel.Text = task.Interval + " time a " + task.Quantifier.ToString();
				//else FrequenzyLabel.Text = task.Interval + " times a " + task.Quantifier.ToString().ToLower();
			}
            /*
			DescriptionLabel.Text = task.Comment;
            DescriptionLabel.TextColor = UIColor.Black;
            if(string.IsNullOrEmpty(task.Comment)) {
                DescriptionLabel.Text = task.Area.Description;
                DescriptionLabel.TextColor = UIColor.FromName("FadeTextColor");
            } */

			
            DescriptionLabel.Text = task.Area.Description;

            if(task.Area.CleaningPlanID == 1) {
                if(task.Frequency != null)
                    TimesCleanedLabel.Text = task.Frequency;
                else 
                    TimesCleanedLabel.Text = task.TimesOfYear + " gange årligt";

                FrequenzyLabel.Text = "Frekvens";

                LastCleanedLabel.Text = task.Comment;

                if (task.SquareMeters > 0)
                    DescriptionLabel.Text += ", " + task.SquareMeters + "m²";

            } else {
                if (task.LastTaskCompleted != null) LastCleanedLabel.Text = "Sidst rengjort den " + task.LastTaskCompleted.CompletedDate.ToString("dd/MM/yy");
                else LastCleanedLabel.Text = "Not cleaned yet";
                TimesCleanedLabel.Text = task.TimesCleanedThisYear + "/" + task.TimesOfYear;
			}
		}
    }
}