using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class CleaningPlanCompletedTasksCell : UITableViewCell
    {
        public CleaningPlanCompletedTasksCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(CleaningTaskCompleted task)
		{
            DescriptionLabel.Text = task.CompletedDate.ToString("HH:mm dd/MM/yy");
            TimeLabel.Text = task.Comment;
            CompletedByLabel.Text = task.CompletedByUser.FirstName + " " + task.CompletedByUser.LastName;
		}
    }
}