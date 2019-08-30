using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibrary.Shared.Observer;
using Rent.Shared.Models;
using RentApp.Shared.Repositories;
using ModuleLibraryiOS.Alert;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using RentAppProject;
using Rent.Shared.ViewModels;
using Rent.Shared.Repositories;

namespace RentApp
{
    public partial class MoreWorkVC : UITableViewController
    {
        public MoreWorkVC(IntPtr handle) : base(handle) { }

        private readonly MoreWorkRepository _moreworkRepository = AppDelegate.ServiceProvider.GetService<MoreWorkRepository>();
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();
        private readonly UserRepository _userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        private readonly UserVM loggedInUser = AppDelegate.ServiceProvider.GetService<UserVM>();

        MoreWork moreWork;
        Location location;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            moreWork = new MoreWork();

            moreWork.LocationID = location.ID;
            moreWork.CreatedByUserID = loggedInUser.ID;

            this.RightNavigationButton("Bestil", (obj) =>
            {
                moreWork.Description = TaskDescription.Text;

                if(moreWork.Description.Length < 5)
                {
                    this.DisplayToast("Skriv længere beskrivelse");
                    return;
                }

                this.DisplayAlert(
                    "Er opgaven udført?",
                    "Hvis opgaven allerede er udført, så tryk 'Udført' og indtast det brugte antal timer. Ellers tryk 'Bestil'.",
                    new System.Collections.Generic.List<(string, Action)> {
                    ("Udført", () => completedWork()),
                    ("Bestil", () => orderWork())}
                );

            });
        }

        void orderWork()
        {
            moreWork.Hours = null;
            sendMoreWork();
        }

        void completedWork()
        {
            bool success = false;
            this.DisplayTextField("Antal brugte timer", "Tast ind hvor mange timer der blev brugt til at udfører opgaven", (text) =>
            {
                success = float.TryParse(text, out float hours);

                if (!success)
                {
                    this.DisplayAlert(
                    "Timer indtastet forker",
                    "Prøv igen",
                    new System.Collections.Generic.List<(string, Action)> {
                        ("Ok", () => completedWork()),
                        ("Nej", null),
                    }
                );
                } else {
                    moreWork.Hours = hours;
                    sendMoreWork();
                }
            });
        }

        void sendMoreWork() {
            _moreworkRepository.Add(moreWork, (addedMoreWork) => {
                this.NavigationController.PopViewController(true);
            }).LoadingOverlay(this);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var index = indexPath.Row;
            if(indexPath.Section == 0)
            {
                if (index == 0)
                {
                    _floorAreaRepository.Plans((obj) => {
                        this.Start<SimplePickerVC>().Setup("Vælg plantype", obj.Select(p => p.Description).ToArray(), (planIndex) => {
                            moreWork.CleaningPlanID = obj[planIndex].ID;
                            TaskType.Text = obj[planIndex].Description;
                        });
                    }).LoadingOverlay(this);
                }
                if (index == 1)
                {
                    this.Start<SimpleDatePickerVC>().Setup("Vælg sidste udførelsesdato", (picker) => {

                    }, (date) => {
                        moreWork.ExpectedCompletedTime = date;
                        DateToComplete.Text = date.ToString("yy-MM-dd");
                    });
                }
                if (index == 2)
                {
                    _userRepository.GetLocationUsers(location, (users) => {
                        this.Start<SimpleTableVC>().Setup("Vælg personale der skal udfører opgaven",
                                                          users.Where(u => u.CustomerID == null).Select(u => (u.FirstName + " " + u.LastName, u.Title, u.ImageLocation)).ToList(),
                                                          (userIndex) => {
                                                              UserToComplete.Text = users.ToList()[userIndex].FirstName + " " + users.ToList()[userIndex].LastName;
                                                              moreWork.UserID = users.ToList()[userIndex].ID;
                                                              this.NavigationController.PopViewController(true);
                                                          });
                    }).LoadingOverlay(this); ;
                }
            }
        }

        public void Setup(Location location)
        {
            this.location = location;
        }
    }
}